CxList methods = Find_Methods();
List<string> methodsNames = new List<string>{		
		"_mbslen",
		"_mbstrlen",
		"_tcsclen",
		"_tcslen",
		"lstrlen",
		"strlen",
		"StrLen",
		"strnlen_s",
		"tcslen",
		"wcslen"
		};
result = methods.FindByShortNames(methodsNames);