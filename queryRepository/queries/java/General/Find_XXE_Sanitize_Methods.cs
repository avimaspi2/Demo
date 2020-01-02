CxList methods = Find_Methods();

CxList sanitizers = All.NewCxList();
//DOM
sanitizers.Add(methods.FindByMemberAccess("DocumentBuilderFactory.setFeature", false));
	//SAX
sanitizers.Add(methods.FindByMemberAccess("SAXParserFactory.setFeature", false));
	// XMLReader 
sanitizers.Add(methods.FindByMemberAccess("XMLReader.setFeature", false));
	// JDOM SAXBuilder
sanitizers.Add(methods.FindByMemberAccess("SAXBuilder.setFeature", false));
	// DOM4J
sanitizers.Add(methods.FindByMemberAccess("SAXReader.setFeature", false));
	// StAX
sanitizers.Add(methods.FindByMemberAccess("XMLInputFactory.setProperty", false));

List<string> esapiSanitizers = new List<string> {
		"encodeForBase64",
		"encodeForCSS",
		"encodeForHTML",
		"encodeForHTMLAttribute",
		"encodeForJavaScript",
		"encodeForURL",
		"encodeForXML",
		"encodeForXMLAttribute"};
// esapi
CxList getESAPI = Get_ESAPI().FindByMemberAccess("Encoder.encode*");
CxList esapiEncode = getESAPI.FindByShortNames(esapiSanitizers);

// org.owasp.encoder.Encode
List<string> methodsOWASP = new List<string> {
		"forCssString",
		"forCssUrl",
		"forHtml",
		"forHtmlAttribute",
		"forHtmlUnquotedAttribute",
		"forUriComponent",
		"forXml",
		"forXmlAttribute"};
CxList owaspEncode = methods.FindByMemberAccess("Encode.for*").FindByShortNames(methodsOWASP);

List<string> reformSanitizers = new List<string> {
		"HtmlAttributeEncode",
		"HtmlEncode",
		"JsString",
		"VbsString",
		"XmlAttributeEncode",
		"XmlEncode"};

CxList reformEncoder = All.FindByType("Reform").GetMembersOfTarget().FindByShortNames(reformSanitizers);

result = sanitizers;
result.Add(esapiEncode);
result.Add(owaspEncode);
result.Add(reformEncoder);
result.Add(All.FindByMemberAccess("BASE64Encoder.encode"));
result.Add(All.FindByMemberAccess("Base64.encode"));
result.Add(All.FindByMemberAccess("HexBin.encode"));