CxList methods = Find_Methods();

// 1 - Direct DB function names
CxList directDbMethods = methods.FindByShortNames(new List<string> {"sybase_fetch_array", "sybase_fetch_assoc", 
	"sybase_fetch_field", "sybase_fetch_object", "sybase_fetch_row","sybase_result"});
/*
CxList sybase = methods.FindByShortName("sybase_*");
CxList sybase_fetch = sybase.FindByShortName("sybase_fetch_*");
CxList directDbMethods =
	sybase_fetch.FindByShortName("sybase_fetch_array") +
	sybase_fetch.FindByShortName("sybase_fetch_assoc") +
	sybase_fetch.FindByShortName("sybase_fetch_field") +
	sybase_fetch.FindByShortName("sybase_fetch_object") +
	sybase_fetch.FindByShortName("sybase_fetch_row") +
	sybase.FindByShortName("sybase_result");
*/	
	result.Add(directDbMethods);