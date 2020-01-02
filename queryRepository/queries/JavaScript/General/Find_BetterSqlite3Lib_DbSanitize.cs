CxList Statements = Find_BetterSqlite3Lib_Statements();

List<string> stMethods = new List<string>{ "get", "all", "iterate" };

CxList stInMethods = Statements.GetMembersOfTarget().FindByShortNames(stMethods);

result = All.GetParameters(stInMethods);