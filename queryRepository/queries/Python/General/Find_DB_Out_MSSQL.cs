CxList allDataInfluencedByConn = Find_DB_Conn_MSSQL();

//1 - Explicit pymsql module fecth functions
result.Add(allDataInfluencedByConn.GetMembersOfTarget().FindByShortName("fetch*"));


//2 - Implicit fecth statements
result.Add(allDataInfluencedByConn.GetFathers().FindByType(typeof(ForEachStmt)));