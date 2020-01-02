CxList methods = Find_Methods();

List<string> methodsNames = new List<string>{		
		"strncpy",
		"_strncpy*",
		"lstrcpyn",
		"_tcsncpy*",
		"_mbsnbcpy*",
		"_wcsncpy*",
		"wcsncpy"
		};

result = methods.FindByShortNames(methodsNames);