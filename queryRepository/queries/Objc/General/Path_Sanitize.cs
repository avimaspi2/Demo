CxList methods = Find_Methods();
CxList memberAccess = Find_MemberAccess();

List<string> sanitizerInstanceProperties = new List<string>{
		"stringByStandardizingPath", "standardizingPath", "pathExtension", "lastPathComponent*",
		"pathComponents*"
		};

result.Add(memberAccess.FindByShortNames(sanitizerInstanceProperties));

List<string> sanitizerMethods = new List<string>
	{
		"*isEqual*", "addingPercentEncoding*", "addingPercentEscapes*", "allKeysForObject*",
		"caseInsensitiveCompare*", "compare*", "components*", "containsObject*", "indexOfObject*", 
		"localizedCaseInsensitiveCompare*", "localizedCompare*", "localizedStandardCompare*", 
		"member*", "objectForKey*", "range*", "replacingCharacters*", 
		"stringByAddingPercentEncodingWithAllowedCharacters*", "stringByAddingPercentEscapesUsingEncoding*", 
		"stringByReplacingCharactersInRange*", "valueForKey*"
		};

result.Add(methods.FindByShortNames(sanitizerMethods));