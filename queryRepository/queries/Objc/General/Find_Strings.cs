result = base.Find_Strings();
// Debugging Identifiers
List<string> debugIdentifiers = new List<string> { 		
		"__FILE__", "__FUNCTION__",
		"#file", "#function"
		};
result.Add(Find_UnknownReference().FindByShortNames(debugIdentifiers));