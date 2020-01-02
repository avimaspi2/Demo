CxList outputs = All.NewCxList();

if(param.Length == 1){

	outputs = param[0] as CxList;
}

CxList methods = Find_Methods() + All.FindByType(typeof(MethodRef));

CxList escapeSanitizers = methods.FindByShortNames(new List<string> {
		"escapeXml*",
		"escapeJavaScript*",
		"escapeHtml*",
		"htmlEscape*",
		"getEscapeXml*",
		"getEscapeJavaScript*",
		"getEscapeHtml*",
		"getHtmlEscape*"});

CxList ibatis = Ibatis();
CxList ibatisSanitizers = ibatis - ibatis.FindByShortName("execute*");

// All replaces that contain \r or \n as first parameter should be removed
CxList replace = methods.FindByShortName("replace*");
CxList replaceEnter = Find_Strings().GetParameters(replace, 0);
replaceEnter = replaceEnter.FindByShortName(@"*[^a-zA-Z]*");
replace = replace.FindByParameters(replaceEnter);
CxList exec = 
	All.FindByMemberAccess("Runtime.exec") + 
	All.FindByMemberAccess("getRuntime.exec") +
	All.FindByMemberAccess("System.exec") +
	All.FindByShortName("safeExec");

CxList setStatus = All.FindByShortName("setStatus");

CxList filter = All.FindByMemberAccess("ResponseUtils.filter");

/* GWT Sanitizer */
CxList GWTSanitizer = All.FindByShortName("asString");
/* GWT */

// esapi
CxList getESAPI = Get_ESAPI();
CxList esAPISanitize = getESAPI.FindByShortName("encodeForJavaScript") +
	getESAPI.FindByShortName("encodeForVBScript") +
	getESAPI.FindByShortName("encodeForCSS");
// esapi

// org.owasp.encoder.Encode
CxList owaspEncoder = All.FindByShortNames(new List<string>{
		"forHtml",
		"forHtmlContent",
		"forHtmlAttribute",
		"forHtmlUnquotedAttribute",
		"forCssString",
		"forCssUrl",
		"forUri",
		"forUriComponent",
		"forXml",
		"forXmlContent",
		"forXmlAttribute",
		"forXmlComment",
		"forCDATA",
		"forJava",
		"forJavaScript",
		"forJavaScriptAttribute",
		"forJavaScriptBlock",
		"forJavaScriptSource"		
		});
// Remove from owasp encoder sanitation if in a JS event, unless encoding for JS.
owaspEncoder -= owaspEncoder.FindByRegex(@"<[^>]+(onclick|ondblclick|onmousedown|onmousemove|onmouseover|onmouseout|onmouseup|onchange|oncontextmenu|oncopy|oncut|onerror|onfocus|onkeydown|onkeypress|onkeyup|onload|onpaste|onreset|onresize|onscroll|onsubmit)\s*=[^>]*for(?!JavaScript)[^>]*>");
	
result = All.NewCxList();
result.Add(Find_XSS_Replace()); 
result.Add(Find_Encode());
result.Add(esAPISanitize);
result.Add(Find_General_Sanitize());
result.Add(escapeSanitizers);
result.Add(GWTSanitizer);
result.Add(Find_Replace_Param());
result.Add(ibatisSanitizers);
result.Add(Find_Parameters());
result.Add(exec);
result.Add(replace);
result.Add(setStatus);
result.Add(filter); 
result.Add(owaspEncoder);



// Response methods not prone to XSS 
CxList response = All.FindByMemberAccess("response.*");
result.Add(response.FindByShortName("setBufferSize"));
result.Add(response.FindByShortName("setCharacterEncoding"));
result.Add(response.FindByShortName("setContentType"));
result.Add(response.FindByShortName("setHeader"));
result.Add(response.FindByShortName("setLocale"));
result.Add(All.FindByMemberAccess("ZipOutputStream.*"));

result -= result.FindByType(typeof(ClassDecl));
result -= result.FindByType(typeof(TypeDecl));
result -= result.FindByType(typeof(MethodDecl));
result -= result.FindByType(typeof(NamespaceDecl));
result -= Find_Strings();
result -= Find_Decode_Encode(outputs);