CxList methods = Find_Methods();

// 1 - Direct DB function names
CxList directDbMethods =
	methods.FindByShortName("ora_do") +
	methods.FindByShortName("ora_exec");
	
	result.Add(directDbMethods);