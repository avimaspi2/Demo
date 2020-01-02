CxList declarators = Find_Declarators();
CxList constants = Find_Constants();

// variable decl which is a constant or artificially created is irrelevant
CxList vars = declarators - declarators.FindAllReferences(constants);
CxList jspDecl = declarators.FindByShortNames(new List<string> {
		"request",
		"response",
		"session",
		"application",
		"pageContext",
		"attr_*"});

vars -= jspDecl;

result = vars;