// Improper Clearing of Heap Memory Before Release ('Heap Inspection')
// this query is looking for storage of password(s) that are stored unencrypted in the memory,
// and so visible to an attacker invoking a memory dump.
// In general, one should not store passwords in an unencrypted form.
// This query finds variables that store passwords in an unencrypted form.

CxList passwords = Find_All_Passwords();

// 1)exclude variables that are all uppercase - usually describes the pattern of the data, such as PASSWORDPATTERN, PASSORDTYPE...
CxList upperCase = All.NewCxList();
foreach (CxList res in passwords)
{
	string name = res.GetName();
	if (name.ToUpper().Equals(name))
	{
		upperCase.Add(res);
	}
}
passwords -= upperCase;

//2) define sanitizers = encryption
CxList sanitizeMethods = All.FindByMemberAccess("KeyStore.setKeyEntry");
sanitizeMethods.Add(All.FindByMemberAccess("KeyStore.SetCertificate"));
sanitizeMethods = All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("SealedObject");
CxList sanitize = All.FindByFathers(sanitizeMethods);
sanitize.Add(Find_Encrypt());

//3) define safe types
string[] safeTypes = {"*.sealedObject", "sealedObject", "*.guardedstring", "guardedstring", "*.KeyStore", "KeyStore"};
passwords -= passwords.FindByTypes(safeTypes);
passwords -= passwords.FindByType(typeof(ConstantDecl));

//4) find assignments of literals
CxList literals = Find_Strings();
literals.Add(All.FindByType(typeof(IntegerLiteral)));
CxList assignLiteral = literals.GetFathers();

//find the declaration statements of passwords
CxList passDecl = passwords.FindByType(typeof(Declarator)); 
passDecl.Add(passwords.FindByType(typeof(FieldDecl))); // property of classes are of FieldDecl type

// find password that is assigned a literal
CxList passAssigned = passwords.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList passAssignLiteral = passAssigned.FindByFathers(assignLiteral);
passAssignLiteral.Add(assignLiteral);

passAssigned -= passAssignLiteral; //remove password that is assigned a literal

//remove password that is assigned a safe object or an encrypted object
passAssigned -= passAssigned.InfluencedBy(sanitize);  
	
result = All.FindDefinition(passAssigned);