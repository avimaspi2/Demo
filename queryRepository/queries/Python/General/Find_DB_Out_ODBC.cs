CxList allDataInfluencedByConn = Find_DB_Conn_ODBC();

//Explicit fetch, fetchone and fetchall
result.Add(allDataInfluencedByConn.GetMembersOfTarget().FindByShortName("fetch*"));