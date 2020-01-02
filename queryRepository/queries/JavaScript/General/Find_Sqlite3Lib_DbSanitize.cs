// Support for Sqlite3 Database Inputs
CxList parameters = Find_Param();
CxList dbs = Find_Sqlite3Lib_Databases();
var dbMembers = new List<string>{"run","get","all","each","exec","prepare"};
CxList dbInMethods = dbs.GetMembersOfTarget().FindByShortNames(dbMembers);
CxList dbInMethodsParams = All.GetParameters(dbInMethods) - All.GetParameters(dbInMethods,0);

CxList dbInMethodCallbacks = dbInMethodsParams.FindByShortName("anony*"); // callbacks
dbInMethodsParams -= dbInMethodCallbacks;
result.Add(dbInMethodsParams);

CxList sqliteStatements = Find_Sqlite3Lib_Statements();
var stmtMembers = new List<string>{"bind","run","get","all","each"};
CxList stmtMethods = sqliteStatements.GetMembersOfTarget().FindByShortNames(stmtMembers);
CxList stmtMethodCallbacks = sqliteStatements.FindByShortName("anony*"); // callbacks
stmtMethods -= stmtMethodCallbacks;
CxList stmtResultCallbacks = All.GetParameters(stmtMethods);
result.Add(stmtResultCallbacks - stmtResultCallbacks .FindByShortName("anony*"));

result -= parameters;