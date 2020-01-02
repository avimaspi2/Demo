CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods =
	methods.FindByShortName("dbx_query");
	
result.Add(directDbMethods);