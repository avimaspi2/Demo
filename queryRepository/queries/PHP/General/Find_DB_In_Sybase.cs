CxList methods = Find_Methods();

// 1 - Direct DB function names
CxList directDbMethods =
	methods.FindByShortName("sybase_query") +
	methods.FindByShortName("sybase_unbuffered_query");
	
	result.Add(directDbMethods);