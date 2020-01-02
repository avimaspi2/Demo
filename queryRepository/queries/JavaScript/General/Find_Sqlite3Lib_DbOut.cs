CxList Databases = Find_Sqlite3Lib_Databases();
CxList Statements = Find_Sqlite3Lib_Statements();

List<string> dbMethods = new List<string>{ "run", "get", "all", "each", "exec" };
List<string> stMethods = new List<string>{ "run", "get", "all", "each" };

CxList dbMethodsRefs = Databases.GetMembersOfTarget().FindByShortNames(dbMethods);
CxList stMethodsRefs = Statements.GetMembersOfTarget().FindByShortNames(stMethods);


CxList dbAccesses = All.NewCxList();
dbAccesses.Add(dbMethodsRefs);
dbAccesses.Add(stMethodsRefs);

result.Add(All.GetParameters(Get_MethodDecl_of_UnknownRef(All.GetParameters(dbAccesses).FindByShortName("anony*"))));