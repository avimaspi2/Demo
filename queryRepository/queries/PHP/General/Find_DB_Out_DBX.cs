CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods = 
	methods.FindByShortName("dbx_fetch_row") ;
result.Add(directDbMethods);