CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods = methods.FindByShortNames(new List<string> {"mssql_bind", "mssql_fetch_array", "mssql_fetch_assoc",
		"mssql_fetch_row","mssql_result"});

/*
CxList mssql = methods.FindByShortName("mssql_*");
CxList directDbMethods = 
	mssql.FindByShortName("mssql_bind") + 
	mssql.FindByShortName("mssql_fetch_array") + 
	mssql.FindByShortName("mssql_fetch_assoc") +
	mssql.FindByShortName("mssql_fetch_row") +
	mssql.FindByShortName("mssql_result") ;
*/
result.Add(directDbMethods);