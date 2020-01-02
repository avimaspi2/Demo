// Improper Clearing of Heap Memory Before Release ('Heap Inspection')
// this query is looking for storage of password(s) that are stored unencrypted in the memory,
// and so visible to an attacker invoking a memory dump.
// In general, one should not store passwords in an unencrypted form.
// This query finds variables that store passwords in an unencrypted form.

CxList passwords = Find_All_Passwords();
CxList methods = Find_Methods();

// remove passwords used as flags (Booleans)
passwords -= passwords.FindByType("Bool");

//2) define sanitizers = encryption
CxList sanitizeMethods = All.FindByMemberAccess("KeyStore.setKeyEntry");
sanitizeMethods.Add(All.FindByMemberAccess("KeyStore.SetCertificate"));
sanitizeMethods = All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("SealedObject");
CxList sanitize = All.FindByFathers(sanitizeMethods);
sanitize.Add(Find_Encrypt());

//3) define safe types
string[] safeTypes = {"*.sealedObject", "sealedObject", "*.guardedstring", "guardedstring", "*.KeyStore", "KeyStore"};
passwords -= passwords.FindByTypes(safeTypes);
passwords -= (passwords * Find_Constants());

//4) find assignments of literals
CxList literals = Find_Strings();
literals.Add(All.FindByType(typeof(IntegerLiteral)));
CxList assignLiteral = literals.GetFathers();

// find password that is assigned a literal
CxList passAssigned = passwords.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList passAssignLiteral = passAssigned.FindByFathers(assignLiteral);
passAssignLiteral.Add(assignLiteral);

passAssigned -= passAssignLiteral; //remove password that is assigned a literal

//remove password that is assigned a safe object or an encrypted object
passAssigned -= passAssigned.InfluencedBy(sanitize);  
  
result = All.FindDefinition(passAssigned);

//Remove char arrays which have reassigned indexes.
CxList indexerRefs = All.FindByType(typeof(IndexerRef));
CxList safePasswords = All.NewCxList();
foreach(CxList iRef in indexerRefs){
	IndexerRef IRefDom = iRef.TryGetCSharpGraph<IndexerRef>();
	if(IRefDom.Target != null){safePasswords.Add(result.FindDefinition(All.FindById(IRefDom.Target.NodeId)));}
}

//Remove passwords which are targets of map functions since they are most likely scrambling them.
CxList mapMethods = methods.FindByShortName("map");
safePasswords.Add(mapMethods.GetTargetOfMembers());

result -= result.FindDefinition(safePasswords);