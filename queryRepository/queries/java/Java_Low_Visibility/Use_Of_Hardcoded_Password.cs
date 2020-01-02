CxList emptyString = Find_Empty_Strings();
CxList nullsString = All.FindByName("null");
CxList psw = Find_Passwords();
CxList allPasswords = Find_All_Passwords();

CxList stringLiterals = Find_Strings();
CxList passwordString = Find_Password_Strings();

// Find password in an initialization operation
CxList pswInLSide = psw.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList pswInLSideDecl = pswInLSide.FindByType(typeof(Declarator));

CxList strLiterals = All.NewCxList();
strLiterals.Add(stringLiterals);
strLiterals -= emptyString;
strLiterals -= nullsString;

//when the hardcoded string includes a space or dot we believe 
//it is not a password string
strLiterals -= strLiterals.FindByName("* *");
strLiterals -= strLiterals.FindByName("*.*");
strLiterals -= strLiterals.FindByName("*/*");
strLiterals -= strLiterals.FindByName("*\\*");

CxList litInRSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);
CxList initializedPassword = pswInLSideDecl.FindByInitialization(litInRSide);

//remove passwords with equal name and contant ==> currPassword = "currPassword";
CxList notHdPass = All.NewCxList();
char[] trimChars = new char[2] { '\'', '"'};
foreach(CxList currPass in initializedPassword)
{
	CxList currStrInLeft = litInRSide.FindInitialization(currPass);
	string strName = currStrInLeft.GetName().Trim(trimChars);
	string passName = currPass.GetName();
	if (passName.Equals(strName))
	{
		notHdPass.Add(currPass);
	}
}
initializedPassword -= notHdPass;

// Find password in an "equals" operation
CxList eq = All.FindByMemberAccess("String.equals");
CxList equalsPassword = strLiterals.GetByAncs(eq * psw.GetMembersOfTarget());

eq *= strLiterals.GetMembersOfTarget();
equalsPassword.Add(psw.GetByAncs(eq));

//equals of type "string".equals(password);
CxList strEQ = strLiterals.GetMembersOfTarget().FindByShortName("equals");
strEQ = psw.GetByAncs(strEQ);
equalsPassword.Add(strEQ);

// Find password in assignments
CxList assignPassword = pswInLSide.GetAncOfType(typeof(AssignExpr));
assignPassword = litInRSide.GetByAncs(assignPassword);

CxList methods = Find_Methods();
CxList connection = methods.FindByShortName("getConnection");
CxList connetionParam2 = All.GetParameters(connection, 2);

CxList connetionParam0 = All.GetParameters(connection, 0);
CxList pwdInFirstConnectionParam = connetionParam0.FindByType(typeof(StringLiteral)).FindByName("*PWD=*");
pwdInFirstConnectionParam.Add(connetionParam0.FindByType(typeof(StringLiteral)).FindByName("*PWD =*"));


// Add also KerberosKey's second parameter as a potentially vulnerable hardcoded parameter
CxList kerberosKeyType = All.FindByType("KerberosKey");
CxList kerberosKey = kerberosKeyType.FindByType(typeof(ObjectCreateExpr));
kerberosKey.Add(kerberosKeyType.FindByType(typeof(Declarator)));

// Get second parameter
CxList KerberosKeyParam1 = All.GetParameters(kerberosKey, 1);

// Add also KerberosPrincipal's second parameter as a potentially vulnerable hardcoded parameter
CxList passwordAuthenticationType = All.FindByType("PasswordAuthentication");
CxList passwordAuthentication = passwordAuthenticationType.FindByType(typeof(ObjectCreateExpr));
passwordAuthentication.Add(passwordAuthenticationType.FindByType(typeof(Declarator)));

// Get second parameter
CxList PasswordAuthenticationParam1 = All.GetParameters(passwordAuthentication, 1);

CxList relevantParams = All.NewCxList();	
relevantParams.Add(KerberosKeyParam1);
relevantParams.Add(connetionParam2);
relevantParams.Add(PasswordAuthenticationParam1);

// Sanitize by binaries such as "+" and by concatenate - could be concatenated with a non hard-coded key, 
// which is OK
CxList bin = Find_BinaryExpr();
bin = bin.FindByShortName("");
CxList concat = All.FindByShortName("concatenate", false);

CxList sanitize = All.NewCxList();
sanitize.Add(bin);
sanitize.Add(concat);

CxList undefinedMethods = methods - methods.FindAllReferences(All.FindDefinition(methods));
sanitize.Add(undefinedMethods);

// Add the parameter itself, or whatever is influencing it
CxList paramsAffectedByString = (relevantParams * strLiterals);
paramsAffectedByString.Add(relevantParams.InfluencedByAndNotSanitized(strLiterals, sanitize));

CxList setPasswordMethod = methods.FindByShortName("setPassword", false);
CxList passwordParams = stringLiterals.GetParameters(setPasswordMethod);
CxList hardcodedPasswordInMethod = setPasswordMethod.DataInfluencedBy(passwordParams);

//Enum members with hardcoded password
CxList enumMembers = Find_EnumMemberDecl();
CxList literalsInEnum = strLiterals.FindByFathers(enumMembers);

//Case 1:
//public enum Credentials { 
//USER("admin:password"), WHITE_LIST("user:password"); 
//}
CxList passwordInEnum = literalsInEnum * passwordString;
passwordInEnum = passwordInEnum.GetFathers();

//Case 2:
//public enum Credentials { 
//PASSWORD("123456"), PWD("user:password"); 
//}
CxList enumWithLiterals = strLiterals.GetFathers() * enumMembers;
passwordInEnum.Add(enumWithLiterals * allPasswords);

// Android password in SharedPreferences
CxList putString = methods.FindByMemberAccess("Editor.putString");
CxList putStringParam0 = All.GetParameters(putString, 0);


CxList StringParam0AndPass = (putStringParam0 * passwordString);

CxList putStringParam0Paswd = putStringParam0.InfluencedBy(passwordString);
putStringParam0Paswd.Add(StringParam0AndPass);

CxList relevantPutString = putStringParam0Paswd.GetAncOfType(typeof(MethodInvokeExpr)) * putString;
CxList passwordValueInPreference = All.GetParameters(relevantPutString, 1);

CxList hardcodedPasswordString = passwordValueInPreference.FindByType(typeof(StringLiteral));

CxList passwordInPreferences = passwordValueInPreference.InfluencedBy(stringLiterals);
passwordInPreferences.Add(hardcodedPasswordString);

result = initializedPassword; 

// ANT build file
result.Add(allPasswords.FindByAssignmentSide(CxList.AssignmentSide.Right).FindByFileName("*build.xml"));

// All
result.Add(equalsPassword);
result.Add(assignPassword);
result.Add(paramsAffectedByString);
result.Add(hardcodedPasswordInMethod);
result.Add(pwdInFirstConnectionParam);
result.Add(passwordInEnum);
result.Add(Password_In_Credentails());
result.Add(passwordInPreferences);

result -= Find_Properties_Files();