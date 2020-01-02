CxList allDataInfluencedByConn = Find_DB_Conn_SQLite();

//Explicit execute functions
result.Add(allDataInfluencedByConn.GetMembersOfTarget().FindByShortName("execute*"));