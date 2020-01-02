CxList Statements = Find_BetterSqlite3Lib_Statements();

List<string> stMethods = new List<string>{ "get", "all", "iterate", "bind", "run" };

CxList stInMethods = Statements.GetMembersOfTarget().FindByShortNames(stMethods);

result.Add(All.GetParameters(stInMethods) - Find_Param());