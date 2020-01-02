CxList methods = Find_Methods();

List<string> allMethods = new List<string>() {
		"memcpy",
		"wmemcpy",
		"_memccpy",
		"memmove",
		"wmemmove",
		"memset",
		"wmemset",
		"memcmp",
		"wmemcmp",
		"memchr",
		"wmemchr"
		};

result = methods.FindByShortNames(allMethods);
result.Add(Find_All_strncpy()); 
result.Add(Find_All_strncat());