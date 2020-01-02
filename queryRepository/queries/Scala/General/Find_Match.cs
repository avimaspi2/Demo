CxList methods = Find_Methods();
result.Add(methods.FindByShortNames(new List<string>{"findAllIn",
		"findFirstIn", "findFirstMatchIn", "findPrefixMatchOf",
		"findPrefixOf", "split", "matches"}));