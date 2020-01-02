CxList methods = Find_Methods();

// org.owasp.encoder.Encode
List<string> methodsOWASP = new List<string> {
		"forCssString",
		"forCssUrl",
		"forHtml",
		"forHtmlAttribute",
		"forHtmlUnquotedAttribute",
		"forXml",
		"forXmlAttribute"};
CxList owaspEncode = methods.FindByMemberAccess("Encode.for*").FindByShortNames(methodsOWASP);

result = owaspEncode;
result.Add(Find_Sanitize());
result.Add(Get_ESAPI().FindByMemberAccess("Encoder.encodeForSQL"));

CxList toRemove = Find_SQL_DB_In();
toRemove.Add(Find_DB());
toRemove.Add(Find_Plain_DB());
toRemove.Add(Find_DB_Heuristic());

result -= toRemove;