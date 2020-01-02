CxList methods = Find_Methods();
CxList allDataInfluencedByConn = Find_DB_Conn_MSSQL();

// 1 - Explicite DB function names
CxList directDbMethods = methods.FindByShortName("MSSQLStoredProcedure.bind");
directDbMethods.Add(methods.FindByShortName("MSSQLConnection.execute*")); 
	
result.Add(directDbMethods);

// Explicit execute methods
result.Add(allDataInfluencedByConn.GetMembersOfTarget().FindByShortName("execute*"));