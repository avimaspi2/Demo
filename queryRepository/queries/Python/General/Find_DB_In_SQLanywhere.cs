CxList allDataInfluencedByConn = Find_DB_Conn_SQLanywhere();

//Explicit execute functions
result.Add(allDataInfluencedByConn.GetMembersOfTarget().FindByShortName("execute*"));