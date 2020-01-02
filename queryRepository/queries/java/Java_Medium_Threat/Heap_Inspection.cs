// Improper Clearing of Heap Memory Before Release ('Heap Inspection')
// this query is looking for storage of password(s) that are stored unencrypted in the memory,
// and so visible to an attacker invoking a memory dump.
// In general, one should not store passwords in an unencrypted form.
// This query finds variables that store passwords in an unencrypted form.

CxList objectCreations = Find_ObjectCreations();
CxList passwords = Find_Passwords_Unsafe();

// remove passwords used as flags (Booleans)
passwords -= passwords.FindByTypes(new string[]{"bool", "boolean", "Cipher"}, false);

//remove types that are defined in scanned code
//the result of heap inspection should be inside of the class
string pattern = @"(?'sep'[\\\/])[Pp]lugins\k<sep>[Jj]ava\k<sep>";
System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(pattern);
CxList typesDefined = Find_ClassDecl();
CxList toRemove = All.NewCxList();
foreach(CxList types in typesDefined){
	try{
		CSharpGraph typeGraph = types.GetFirstGraph();
		string path = typeGraph.LinePragma.FileName;
		if(re.IsMatch(path)){
			toRemove.Add(types);
		}
	}
	catch(Exception){}
}
typesDefined -= toRemove;
CxList typeRefs = Find_TypeRef();
CxList allRefs = typeRefs.FindAllReferences(typesDefined);
CxList variables = allRefs.GetFathers();
CxList passwordDeclarators = passwords.FindByType(typeof(Declarator));
CxList passwordVariables = passwordDeclarators.GetAncOfType(typeof(FieldDecl));
passwordVariables.Add(passwordDeclarators.GetAncOfType(typeof(VariableDeclStmt)));
passwordVariables.Add(passwordDeclarators.GetAncOfType(typeof(ConstantDecl)));
passwords -= passwords.GetByAncs(passwordVariables * variables);

//2) define sanitizers = encryption
CxList sanitizeMethods = All.FindByMemberAccess("KeyStore.setKeyEntry");
sanitizeMethods.Add(All.FindByMemberAccess("KeyStore.SetCertificate"));
sanitizeMethods = objectCreations.FindByShortName("SealedObject");
CxList sanitize = All.FindByFathers(sanitizeMethods);
sanitize.Add(Find_Encrypt());

//3) define safe types
string[] safeTypes = {"*.sealedObject", "sealedObject",
	"*.guardedstring", "guardedstring",
	"*.KeyStore", "KeyStore",	
	"JPanel", "JPasswordField"};
passwords -= passwords.FindByTypes(safeTypes);
passwords -= (passwords * Find_Constants());

//4) find assignments of literals
CxList literals = Find_Strings();
literals.Add(Find_IntegerLiterals());
CxList assignLiteral = literals.GetFathers();

// find password that is assigned a literal
CxList passAssigned = passwords.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList passAssignLiteral = passAssigned.FindByFathers(assignLiteral);
passAssignLiteral.Add(assignLiteral);

//remove assign literals that are in condition with null validation
//ex. if (password == null) password = ""
CxList conditions = Find_Conditions();
CxList nullLiteral = base.Find_NullLiteral();
CxList nullInCondition = nullLiteral.GetByAncs(conditions);
CxList nullValidations = nullInCondition.GetAncOfType(typeof(IfStmt));
nullValidations.Add(nullInCondition.GetAncOfType(typeof(TernaryExpr)));
passAssignLiteral -= passAssignLiteral.GetByAncs(nullValidations);

passAssigned -= passAssignLiteral; //remove password that is assigned a literal
passAssigned -= All.FindDefinition(passAssignLiteral);

//remove password that is assigned a safe object or an encrypted object
passAssigned -= passAssigned.InfluencedBy(sanitize);  
  
result = All.FindDefinition(passAssigned);

//Remove char arrays which have reassigned indexes.
CxList indexerRefs = Find_IndexerRefs();
CxList safePasswords = All.NewCxList();
foreach(CxList iRef in indexerRefs){
	IndexerRef IRefDom = iRef.TryGetCSharpGraph<IndexerRef>();
	if(IRefDom.Target != null){safePasswords.Add(result.FindDefinition(All.FindById(IRefDom.Target.NodeId)));}
}

result -= result.FindDefinition(safePasswords);

CxList fillSanitizer = All.FindByMemberAccess("Arrays.fill");
CxList fillParameters = All.GetParameters(fillSanitizer, 0);

result -= result.FindDefinition(fillParameters);