// Find all strings
CxList strings = Find_Strings();
// Get all the relevant digest algorithms
CxList digestStrings = strings.FindByShortNames(new List<string> {
		"\"SHA*",
		"\"MD2*",
		"\"MD5*"});

result = digestStrings;