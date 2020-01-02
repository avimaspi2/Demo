CxList Databases = Find_BetterSqlite3Lib_Databases();
CxList Statements = Find_BetterSqlite3Lib_Statements();

List<string> dbMethods = new List<string>{ "pragma", "exec", "transaction", "prepare" };
List<string> stMethods = new List<string>{ "get", "all", "iterate", "bind" };

CxList dbInMethods = Databases.GetMembersOfTarget().FindByShortNames(dbMethods);
CxList stInMethods = Statements.GetMembersOfTarget().FindByShortNames(stMethods);

result = All.GetParameters(dbInMethods, 0);
result.Add(All.GetParameters(stInMethods));