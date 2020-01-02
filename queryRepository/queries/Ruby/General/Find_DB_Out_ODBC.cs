CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods = 
	methods.FindByShortName("odbc_fetch_array") + 
	methods.FindByShortName("odbc_fetch_object") + 
	methods.FindByShortName("odbc_foreignkeys") +
	methods.FindByShortName("odbc_result") ;
result.Add(directDbMethods);

// identify methods which one of their parameters are used for output.
// Then identify the paramenters themselves and then ...
CxList variableUpdatedDbMethod = 
	methods.FindByShortName("odbc_fetch_into") ;