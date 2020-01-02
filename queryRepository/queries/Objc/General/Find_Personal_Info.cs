List <string> personalInfo = new List<string>{"*Credit*", "*credentials*", "*secret*",
		"*Account*", "*SSN", "SSN*",
		"*SocialSecurity*", "auth*"};
CxList tempResult = All.FindByShortNames(personalInfo, false);

List <string> toRemove = new List<string>{"*className*", "credits*"};

tempResult -= All.FindByShortNames(toRemove, false);

tempResult.Add(Find_Passwords());

CxList passwordToRemove = All.NewCxList();
passwordToRemove.Add(tempResult.FindByType(typeof(Param)));
passwordToRemove.Add(tempResult.FindByType(typeof(MethodInvokeExpr)));
passwordToRemove.Add(tempResult.FindByType(typeof(StringLiteral)));
passwordToRemove.Add(tempResult.FindByType(typeof(MethodRef)));
passwordToRemove.Add(tempResult.FindByType(typeof(ThisRef)));
passwordToRemove.Add(tempResult.FindByType(typeof(BaseRef)));

tempResult -= passwordToRemove;

//Add output of Keychain (secure storage on iOS)
CxList methods = Find_Methods();
CxList ur = Find_UnknownReference();
CxList keychainOut = methods.FindByShortName(@"SecItemCopyMatching");

CxList keychainParam = Find_Param();
keychainParam.Add(ur);

tempResult.Add(keychainParam.GetParameters(keychainOut, 1));

//Add unique identifier of current device
List<string> uniqueIdentifiers = new List<string> {"uniqueIdentifier", "identifierForVendor"};
tempResult.Add(All.FindByMemberAccess("UIDevice.currentDevice").GetMembersOfTarget().FindByShortNames(uniqueIdentifiers));

tempResult.Add(tempResult.GetMembersOfTarget());
tempResult.Add(tempResult.GetMembersOfTarget());
tempResult.Add(tempResult.GetMembersOfTarget());

// remove FieldDecl (swift) --- we have already the Declarator
tempResult -= tempResult.FindByType(typeof(FieldDecl)); 

result = tempResult;

CxList refAndMembers = Find_MemberAccess();
refAndMembers.Add(ur);

// Add SecAddSharedWebCredential and SecRequestSharedWebCredential methods' second parameter
CxList SecAddSharedWebCredential = methods.FindByShortName("SecAddSharedWebCredential");
CxList SecAddSharedWebCredentialAccount = All.GetParameters(SecAddSharedWebCredential, 1);
CxList SecRequestSharedWebCredential = methods.FindByShortName("SecRequestSharedWebCredential");
SecAddSharedWebCredentialAccount.Add(All.GetParameters(SecRequestSharedWebCredential, 1));
// Cases when converting NSString to CFString in the method invoke
CxList castExpr = SecAddSharedWebCredentialAccount.FindByType(typeof(CastExpr));
SecAddSharedWebCredentialAccount.Add(refAndMembers.FindByFathers(castExpr));

// Add the first parameter of LAContext setCredential:type: and NSURLCredentialStorage setCredential:forProtectionSpace:
CxList LAContext = refAndMembers.FindByType("LAContext");
CxList setCredentialMerhods = methods.FindByShortName("setCredential:forProtectionSpace:");
setCredentialMerhods.Add(LAContext.GetMembersOfTarget().FindByShortName("setCredential:type:"));
SecAddSharedWebCredentialAccount.Add(All.GetParameters(setCredentialMerhods, 0));

// Seperate the results which are methods - as to not add their references
CxList SecAddSharedWebCredentialMethods = SecAddSharedWebCredentialAccount * methods;
SecAddSharedWebCredentialAccount -= SecAddSharedWebCredentialMethods;
result.Add(SecAddSharedWebCredentialMethods);

SecAddSharedWebCredentialAccount -= Find_Null_Literals();
result.Add(All.FindAllReferences(SecAddSharedWebCredentialAccount));