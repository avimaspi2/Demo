CxList settings = Find_Android_Settings();
CxList xmlConfig = settings.FindByMemberAccess("APPLICATION", "ANDROID_NETWORKSECURITYCONFIG").GetAssigner();
if (xmlConfig.Count > 0) {
	// Try to get xml name
	string name = "*.xml";
	try {
		name = xmlConfig.TryGetCSharpGraph<StringLiteral>().Text.Replace("@xml/", "") + ".xml";
	} catch (Exception e) {}
	result = cxXPath.FindXmlNodesByLocalName(name, 524288, "base-config", true, "cleartextTrafficPermitted", "false");
} else {
	CxList falseLiteralsManifest = settings.FindByShortName("false");
	CxList clearTextTraffic = settings.FindByMemberAccess("APPLICATION", "ANDROID_USESCLEARTEXTTRAFFIC");
	result = (clearTextTraffic.GetAssigner() * falseLiteralsManifest);
}