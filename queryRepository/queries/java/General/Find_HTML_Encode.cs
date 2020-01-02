CxList methods = Find_Methods();

CxList encode = methods.FindByName("*.htmlEncode");
encode.Add(methods.FindByMemberAccess("HtmlEncoder.encode"));
encode.Add(methods.FindByName("*HtmlEncoder.encode"));
encode.Add(methods.FindByMemberAccess("ServletResponse.encode*"));
encode.Add(methods.FindByShortName("*htmlEncode*", false));


CxList encodeReferMethods = encode.FindByType(typeof(MemberAccess));
encodeReferMethods.Add(encode.FindByType(typeof(MethodInvokeExpr)));

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


result = encodeReferMethods;
result.Add(owaspEncode);