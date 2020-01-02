/*
 	Find_DB
 	https://golang.org/pkg/database/sql/
 	All DB output comes from 'Scan' method
	whether it comes from the result of a query (*Rows, *Row) 
 	prepared statements (*Stmt) or Transactions (*Tx)
*/
CxList sql = All.NewCxList();
CxList sqldb = All.NewCxList();
CxList db = All.NewCxList();
CxList content = All.NewCxList();
CxList query = All.NewCxList();
CxList references = All.NewCxList();
CxList assigns = All.NewCxList();
CxList inline = All.NewCxList();
CxList unknownReferences = Find_UnknownReferences();

sql.Add(Find_Members_Database());
db.Add(unknownReferences.FindAllReferences(sql.GetFathers().GetAssignee()).GetMembersOfTarget());

sqldb.Add(All.FindByPointerTypes(new string[]{"sql.DB"}));
db.Add(All.FindAllReferences(sqldb).GetMembersOfTarget());
CxList preparedStmts = unknownReferences.FindAllReferences(db.FindByShortName("Prepare*").GetFathers().GetAssignee());
query.Add(preparedStmts.GetMembersOfTarget().FindByShortName("Query*"));
query.Add(db.FindByShortName("Query*"));
references.Add(All.FindAllReferences(query));
inline.Add(references.GetMembersOfTarget());
assigns.Add(references.GetAssignee());

content.Add(query);
content.Add(references);
content.Add(inline);
content.Add(assigns);
content.Add(All.FindAllReferences(assigns).GetMembersOfTarget());

string[] relevantTypes = new string[] {"sql.Rows", "sql.Row", "sql.Stmt", "sql.Tx"};
CxList allSqlRows = unknownReferences.FindByPointerTypes(relevantTypes);
CxList allMembersOfSqlRows = allSqlRows.GetMembersOfTarget();
CxList transactionQueries = allMembersOfSqlRows.FindByShortName("Query*");
CxList transactionReferences = unknownReferences.FindAllReferences(transactionQueries.GetAssignee());
content.Add(transactionQueries.GetMembersOfTarget());
content.Add(transactionReferences.GetMembersOfTarget());
CxList scan = content.FindByShortName("Scan");

CxList scanArgs = All.GetParameters(scan);
CxList args = unknownReferences.GetByAncs(scanArgs);

//add db outs from PostGres ORM go-pg
CxList gopgORM = Find_PostGres_ORM();
CxList modelCalls = gopgORM.FindByShortName("Model");
CxList modelCallsArguments = All.GetParameters(modelCalls, 0);
CxList modelContextCalls = gopgORM.FindByShortName("ModelContext");
modelCallsArguments.Add(All.GetParameters(modelContextCalls, 1));
//because of addresses in arguments.
modelCallsArguments.Add(All.GetByAncs(modelCallsArguments));

CxList modelsSelected = modelCallsArguments.DataInfluencingOn(gopgORM.FindByShortName("Select"));

result.Add(modelsSelected.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly));
result.Add(args);
result.Add(scan);

//add the results from Cassandra
result.Add(Find_DB_Out_Cassandra());