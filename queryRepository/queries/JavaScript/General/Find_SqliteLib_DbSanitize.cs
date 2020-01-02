CxList Databases = Find_SqliteLib_Databases();
CxList Statements = Find_SqliteLib_Statements();

List <string> dbMethods = new List<string>{ "run", "get", "all", "each", "exec", "prepare" }; 
List <string> stMethods = new List<string>{ "run", "get", "bind", "all", "each" };

CxList dbIns = Databases.GetMembersOfTarget().FindByShortNames(dbMethods);
result.Add((All.GetParameters(dbIns)-All.GetParameters(dbIns,0)) - Find_Param());

CxList stIns = Statements.GetMembersOfTarget().FindByShortNames(stMethods);
result.Add(All.GetParameters(stIns) - Find_Param());