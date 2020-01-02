// Memory Deallocation

// Find all method calls
CxList methods = Find_Methods();

// Find Memory Deallocation
var possibleDeallocations = new List<string>{
		"free",
		"delete",
		"wxDELETE",
		"HeapFree",
		"LocalFree",
		"GlobalFree",
		"closedir"
		};

result = methods.FindByShortNames(possibleDeallocations);