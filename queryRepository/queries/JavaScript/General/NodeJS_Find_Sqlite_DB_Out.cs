CxList methodDecl = Find_MethodDecls();
CxList ur = Find_UnknownReference();

//Gets all the parameters used in a DB's method
CxList queriesParams = ur.GetParameters(NodeJS_Find_Sqlite_DB_In());

//Gets the anonymous functions among them
CxList callbackClass = Get_Class_Of_Anonymous_Ref(queriesParams);

CxList constructor = methodDecl.FindByShortName(callbackClass).GetByAncs(callbackClass);

//Gets the parameter that represent the result
result = All.GetParameters(constructor, 1);