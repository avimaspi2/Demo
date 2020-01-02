// Find_DatabaseSql_Sanitizers

CxList databaseSqlMethods = Find_Members_Database();
CxList assignees = databaseSqlMethods.GetFathers().GetAssignee();

// db.Prepare or db.PrepareContext
CxList references = All.FindAllReferences(assignees);
CxList members = references.GetMembersOfTarget();

CxList sndOrderAssignees = members.GetFathers().GetAssignee();
CxList members2 = All.FindAllReferences(sndOrderAssignees).GetMembersOfTarget();
members.Add(members2);
members = members.ReduceFlowByPragma();

CxList databaseSql = members.FindByShortName("Prepare*");

CxList prepared = All.FindAllReferences(databaseSql.GetFathers().GetAssignee());
prepared.Add(prepared.GetMembersOfTarget());

// the Stmt variables are sanitizers because are obtained from a Prepared
string[] relevantTypes = new string[] {"sql.Stmt"};
CxList variablesOfPointersToRelevantTypes = All.FindByPointerTypes(relevantTypes);
prepared.Add(variablesOfPointersToRelevantTypes);
prepared.Add(variablesOfPointersToRelevantTypes.GetMembersOfTarget());

List<string> memberNames = new List<string> {
		"Exec*",	// db.Exec or db.ExecContext
		"Query*"};	// db.Query or db.QueryContext or db.QueryRow or db.QueryRowContext 
CxList dbMembers = members.FindByShortNames(memberNames);

// Search members with two or more parameters since these are sanitizers.
CxList allParameters = All.GetParameters(dbMembers);
CxList firstParameters = All.GetParameters(dbMembers, 0);
CxList sanitizedParameters = allParameters - firstParameters;

databaseSql.Add(dbMembers.FindByParameters(sanitizedParameters));
databaseSql.Add(sanitizedParameters);

//remove connection opening arguments (FP)
CxList methods = Find_Methods();
CxList openSQL = methods.FindByMemberAccess("\"database/sql\".Open");
openSQL.Add(methods.FindByName("*.sql.Open"));

CxList dbIns = Find_DB_In();
CxList sanitizedSecond = dbIns.FindByShortNames(new List<string>{"Exec", "ExecOne"});
CxList sanitizedParam = All.GetParameters(sanitizedSecond, 1);
CxList sanitizedThird = dbIns.FindByShortNames(new List<string>{"Query", "QueryOne", "ExecContext", "ExecOneContext"});
sanitizedParam.Add(All.GetParameters(sanitizedThird, 2));

result = Find_Integers();
result.Add(sanitizedParam);
result.Add(prepared);

//sanitizer from cassandra
result.Add(Find_DB_Sanitize_Cassandra());
result.Add(Find_WhiteListSanitizers());

result.Add(All.GetParameters(openSQL));