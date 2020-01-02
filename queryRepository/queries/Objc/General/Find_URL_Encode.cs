CxList methods = Find_Methods();

List<string> methodsNames = new List<string> {
		"stringByAddingPercentEscapesUsingEncoding*",
		"addingPercentEscapes*",
		//Add a heuristic check
		"*urlencode*"
		};

result = methods.FindByShortNames(methodsNames,false);