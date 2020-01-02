CxList allDataInfluencedByConn = Find_DB_Conn_OJDBC();

CxList methods = All.FindAllReferences((allDataInfluencedByConn)).GetMembersOfTarget();

//Explicit functions
result.Add(methods.FindByShortName("execute*"));