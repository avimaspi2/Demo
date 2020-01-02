CxList methods = Find_Methods();

List<string> allMethods = new List<string>() {
		"strncpy",
		"lstrcpy",
		"wcscpy",
		"_tcscpy",
		"_mbscpy",
		"CopyMemory", // Do we really want this one???
		"strcat",
		"lstrcat"
		};

result = methods.FindByShortNames(allMethods);