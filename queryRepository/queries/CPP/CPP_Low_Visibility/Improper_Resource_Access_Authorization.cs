// Finds the resources accessed without user authorization validation

// database and file accesses
CxList dataAccess = Find_DB();
dataAccess.Add(Find_IO());
CxList safeDataAccess = dataAccess.FindByType(typeof(Param));
safeDataAccess.Add(dataAccess.FindByType(typeof(NullLiteral)));
dataAccess -= safeDataAccess;
 
// conditions that make use of heuristic words
CxList conditions = Get_Conditions();
CxList potential_check = conditions.FindByShortNames(new List<string>{
		"*allow", "*allows", "*allowed", "*authoriz*", 
		"*deny", "*permission*"}, false);

CxList ifStmtCheck = potential_check.GetAncOfType(typeof(IfStmt));

CxList dataAccessCheck = dataAccess.GetByAncs(ifStmtCheck);
result = dataAccess - dataAccessCheck;