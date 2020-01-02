CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods =
	methods.FindByShortName("odbc_do") + 
	methods.FindByShortName("odbc_exec") + 
	methods.FindByShortName("odbc_execute"); 
	
result.Add(directDbMethods);