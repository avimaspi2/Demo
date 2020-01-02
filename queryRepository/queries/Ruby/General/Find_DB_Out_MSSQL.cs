CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods = 
	methods.FindByShortName("mssql_bind") + 
	methods.FindByShortName("mssql_fetch_array") + 
	methods.FindByShortName("mssql_fetch_assoc") +
	methods.FindByShortName("mssql_fetch_row") +
	methods.FindByShortName("mssql_result") ;

result.Add(directDbMethods);