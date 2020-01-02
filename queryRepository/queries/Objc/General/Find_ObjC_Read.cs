// Reviewed but not verified by source examples
CxList methods = Find_Methods();

List<string> methodsNames = new List<string>{
		"*WithContentsOfFile:*",
		"contentsAtPath:*",
		"unarchiveObjectWithFile:*",
		"regularFileContents:",
		"serializedRepresentation:"
		};

CxList read = methods.FindByShortNames(methodsNames);

result = read;