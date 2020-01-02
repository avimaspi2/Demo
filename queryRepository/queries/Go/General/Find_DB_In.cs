CxList methods = Find_Methods();
CxList unknownRefs = Find_UnknownReferences();

// Find the SQL starting point
CxList openSQL = methods.FindByMemberAccess("\"database/sql\".Open");
openSQL.Add(methods.FindByName("*.sql.Open"));

//Get PostGres ORM related mthods
CxList postGresDB = Find_PostGres_ORM();

// Find the statements:
CxList db = All.FindAllReferences(openSQL).GetAssignee();
CxList dbOcurrences = All.FindAllReferences(db);
List <string> stmtExecutionMethods = new List<string> {
		"Exec", "Query", "QueryRowContext", "QueryContext", "ExecContext", 
		"ExecOne", "ExecOneContext"};
CxList statements = dbOcurrences.GetMembersOfTarget().FindByShortName("Prepare");

// Find all Execs (may have simple queries that shouldn't be considered as DB_Output)
CxList statementsAssignee = All.FindAllReferences(statements).GetAssignee();
CxList statementsOcurrences = All.FindAllReferences(statementsAssignee);
CxList dbTargets = statementsOcurrences.GetMembersOfTarget();

dbTargets.Add(postGresDB);
CxList execsStmt = dbTargets.FindByShortNames(stmtExecutionMethods);
CxList execsDb = dbOcurrences.GetMembersOfTarget().FindByShortNames(stmtExecutionMethods);

/************************ Catch SQL.Query calls ***************************
 * Note that all the SQL.Query except for the SELECT statements are DB in *
 **************************************************************************/
CxList dbQuery = dbOcurrences.GetMembersOfTarget().FindByShortNames(stmtExecutionMethods).ReduceFlowByPragma();
CxList relevantAncs = Find_Strings();
relevantAncs.Add(unknownRefs);
CxList allRelevantParams = relevantAncs.GetByAncs(Find_Param().GetParameters(dbQuery));
CxList allSelectRelevantParams = All.NewCxList();
allSelectRelevantParams.Add(allRelevantParams.FindByAbstractValue(abstractValue => {
	if(abstractValue is StringAbstractValue) {
		return (abstractValue as StringAbstractValue).Content.ToLower().Contains("select");
	}
	
	return false;
	}));
dbQuery -= allSelectRelevantParams.GetAncOfType(typeof(MethodInvokeExpr));

//Using explicit types when available
CxList sqlMethods = methods.FindByMemberAccess("sql.DB", "*");

//add postgres db ins
List <string> execMethods = new List<string> {"Insert", "CreateTable"};
result = execsStmt;
result.Add(execsDb);
result.Add(postGresDB.FindByShortNames(execMethods));
result.Add(sqlMethods.FindByShortNames(stmtExecutionMethods));
result.Add(dbQuery);

//add the results from Cassandra
result.Add(Find_DB_In_Cassandra());
result = result.ReduceFlowByPragma();

//Find uses of newStatement in Fprintf methods
CxList newStatementDef = methods.FindByShortName("newStatement").GetAssignee();
CxList newStatementRefs = unknownRefs.FindAllReferences(newStatementDef);
CxList fprintfRefs = methods.FindByShortName("Fprintf");
CxList fprintfParams = newStatementRefs.GetMembersOfTarget().FindByShortName("Cmd").GetParameters(fprintfRefs, 0);
CxList fprintfCalls = fprintfRefs.FindByParameters(fprintfParams.GetTargetOfMembers());
result.Add(fprintfCalls);