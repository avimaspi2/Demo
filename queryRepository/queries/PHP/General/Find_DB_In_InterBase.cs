CxList methods = Find_Methods();

// 1 - Explicite DB function names
//CxList ibase = methods.FindByShortName("ibase_*");

CxList directDbMethods = methods.FindByShortNames(new List<string> {"ibase_query", "ibase_execute", "ibase_prepare" });
/*
CxList directDbMethods =
	ibase.FindByShortName("ibase_query") +
	ibase.FindByShortName("ibase_execute") +
	ibase.FindByShortName("ibase_prepare");
*/	
result.Add(directDbMethods);