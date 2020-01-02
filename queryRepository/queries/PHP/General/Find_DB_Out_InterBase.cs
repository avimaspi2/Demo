CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods = methods.FindByShortNames(new List<string> {"ibase_fetch_assoc", "ibase_fetch_object", "ibase_fetch_row"});
/*
CxList ibase_fetch = methods.FindByShortName("ibase_fetch_*");
CxList directDbMethods = 
	ibase_fetch.FindByShortName("ibase_fetch_assoc") +
	ibase_fetch.FindByShortName("ibase_fetch_object") +
	ibase_fetch.FindByShortName("ibase_fetch_row");
*/
result.Add(directDbMethods);