CxList allDataInfluencedByConn = Find_DB_Conn_SAP();

CxList methods = All.FindAllReferences((allDataInfluencedByConn)).GetMembersOfTarget();

//Explicit functions
result.Add(methods.FindByShortName("sql*"));
result.Add(methods.FindByShortName("prepare*"));
result.Add(methods.FindByShortName("execute*"));