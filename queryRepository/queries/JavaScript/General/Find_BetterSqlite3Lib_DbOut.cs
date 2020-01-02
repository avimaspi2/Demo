CxList Databases = Find_BetterSqlite3Lib_Databases();
CxList Statements = Find_BetterSqlite3Lib_Statements();

List<string> dbMethods = new List<string>{ "pragma" };
List<string> stMethods = new List<string>{ "get", "all", "iterate", "run" };

CxList dbInMethods = Databases.GetMembersOfTarget().FindByShortNames(dbMethods);
CxList stInMethods = Statements.GetMembersOfTarget().FindByShortNames(stMethods);

result = dbInMethods;
result.Add(stInMethods);