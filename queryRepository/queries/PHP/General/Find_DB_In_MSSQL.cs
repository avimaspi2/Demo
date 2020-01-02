CxList methods = Find_Methods();

// 1 - Explicite DB function names
//CxList mssql = methods.FindByShortName("mssql_*");
CxList directDbMethods = methods.FindByShortNames(new List<string> {"mssql_bind", "mssql_execute", "mssql_query" });
/*
CxList directDbMethods =
	mssql.FindByShortName("mssql_bind") + 
	mssql.FindByShortName("mssql_execute") + 
	mssql.FindByShortName("mssql_query") ;
*/	
result.Add(directDbMethods);