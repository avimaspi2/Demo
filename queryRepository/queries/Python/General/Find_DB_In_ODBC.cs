CxList allDataInfluencedByConn = Find_DB_Conn_ODBC();

//Explicit execute functions
result.Add(allDataInfluencedByConn.GetMembersOfTarget().FindByShortName("execute*"));