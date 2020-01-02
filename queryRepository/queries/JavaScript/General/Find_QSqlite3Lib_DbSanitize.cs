// Support for Sqlite3 Database Inputs
CxList parameters = Find_Param();
CxList dbs = Find_QSqlite3Lib_Databases();
List<string> dbMembers = new List<string>{"run","get","all","each","exec","prepare"};
CxList dbInMethods = dbs.GetMembersOfTarget().FindByShortNames(dbMembers);
CxList dbInMethodsParams = All.GetParameters(dbInMethods) - All.GetParameters(dbInMethods, 0);

CxList dbInMethodCallbacks = dbInMethodsParams.FindByShortName("anony*"); // callbacks
dbInMethodsParams -= dbInMethodCallbacks;
result.Add(dbInMethodsParams);

CxList sqliteStatements = Find_QSqlite3Lib_Statements();
List<string> stmtMembers = new List<string>{"bind","run","get","all","each"};
CxList stmtMethods = sqliteStatements.GetMembersOfTarget().FindByShortNames(stmtMembers);
CxList stmtMethodCallbacks = sqliteStatements.FindByShortName("anony*"); // callbacks
stmtMethods -= stmtMethodCallbacks;
result.Add(All.GetParameters(stmtMethods));

result -= parameters;