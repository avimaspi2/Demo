// Memory Allocation

// Find all method calls
CxList methods = Find_Methods();

List<string> allMethods = new List<string> {
		"malloc","realloc",
		"calloc","strdup",
	 };

// Find memory allocation
result = methods.FindByShortNames(allMethods);