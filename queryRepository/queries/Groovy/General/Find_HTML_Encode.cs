CxList encode = All.FindByName("*.htmlEncode") + 
	All.FindByMemberAccess("HtmlEncoder.encode") + 
	All.FindByName("*HtmlEncoder.encode") + 
	All.FindByMemberAccess("ServletResponse.encode*") +
	All.FindByShortName("*htmlEncode*", false);


encode = encode.FindByType(typeof(MemberAccess)) + encode.FindByType(typeof(MethodInvokeExpr));

CxList esapiEncode = Get_ESAPI().FindByMemberAccess("Encoder.encodeForHTMLAttribute")
	+ Get_ESAPI().FindByMemberAccess("Encoder.encodeForHTML");

result = encode + esapiEncode;