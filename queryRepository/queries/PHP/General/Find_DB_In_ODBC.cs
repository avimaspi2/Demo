CxList methods = Find_Methods();

// 1 - Explicite DB function names
//CxList odbc = methods.FindByShortName("odbc_*");

CxList directDbMethods = methods.FindByShortNames(new List<string> {"odbc_do", "odbc_exec", "odbc_execute" });
/*
CxList directDbMethods =
	odbc.FindByShortName("odbc_do") + 
	odbc.FindByShortName("odbc_exec") + 
	odbc.FindByShortName("odbc_execute"); 
*/	
result.Add(directDbMethods);