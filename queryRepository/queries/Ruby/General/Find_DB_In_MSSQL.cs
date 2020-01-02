CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods =
	methods.FindByShortName("mssql_bind") + 
	methods.FindByShortName("mssql_execute") + 
	methods.FindByShortName("mssql_query") ;
	
result.Add(directDbMethods);