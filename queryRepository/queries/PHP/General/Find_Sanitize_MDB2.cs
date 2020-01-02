// Find_Sanitize_MDB2
CxList methods = Find_Methods();

CxList connections = 
	methods.FindByMemberAccess("MDB2.singleton") +
	methods.FindByMemberAccess("MDB2.factory") +
	methods.FindByMemberAccess("MDB2.connect");

CxList sanitizers = methods.FindByShortNames(new List<string>
	{"_skipDelimitedStrings", "quoteIdentifier", "escape", "quote","bindParam","bindParamArray","bindValue","bindValueArray"});
//	methods.FindByShortName("_skipDelimitedStrings") +
//	methods.FindByShortName("quoteIdentifier") +
//	methods.FindByShortName("escape") +
//	methods.FindByShortName("quote") +
//	methods.FindByShortName("bindParam") +
//	methods.FindByShortName("bindParamArray") +
//	methods.FindByShortName("bindValue") +
//	methods.FindByShortName("bindValueArray");

result = sanitizers.DataInfluencedBy(connections);