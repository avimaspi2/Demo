CxList jspCode = Find_Jsp_Code();
CxList jspTags = Find_Output_Tags();
CxList methods = Find_Methods();
CxList jspMethods = methods * jspCode;

CxList escapeSanitizers = 
	methods.FindByMemberAccess("StringEscapeUtils.escapeXml") +
	methods.FindByMemberAccess("StringEscapeUtils.escapeJavaScript") +
	methods.FindByMemberAccess("StringEscapeUtils.escapeHtml") +
	methods.FindByMemberAccess("HtmlUtils.htmlEscape*") +
	// Add Checkmarx artificially created getters in jsp
	methods.FindByMemberAccess("StringEscapeUtils.getEscapeXml") +
	methods.FindByMemberAccess("StringEscapeUtils.getEscapeJavaScript") +
	methods.FindByMemberAccess("StringEscapeUtils.getEscapeHtml") +
	methods.FindByMemberAccess("HtmlUtils.getHtmlEscape*");

CxList jspSanitizers = jspCode.GetByAncs(jspTags);

CxList mvcSanitizers = jspCode.FindByMemberAccess("c_out.*") + jspCode.FindByMemberAccess("c_param.value");
mvcSanitizers = mvcSanitizers.FindByMemberAccess("response.write") +
	mvcSanitizers.GetMembersOfTarget().FindByMemberAccess("response.write") +
	mvcSanitizers.GetMembersOfTarget().GetMembersOfTarget().FindByMemberAccess("response.write");
CxList beanWriteSanitizers = jspCode.FindByMemberAccess("bean_write.response").GetMembersOfTarget().FindByMemberAccess("response.write");
jspSanitizers.Add(jspCode.GetByAncs(mvcSanitizers + beanWriteSanitizers));

CxList jspEscaped = jspCode.FindByMemberAccess("cx_escFalse.*");
jspSanitizers -= jspCode.GetByAncs(jspEscaped.GetFathers().GetAncOfType(typeof(MethodInvokeExpr)) * beanWriteSanitizers);
jspSanitizers -= jspCode.GetByAncs(jspEscaped.GetAncOfType(typeof(MethodInvokeExpr)));

CxList ibatis = Ibatis();
CxList ibatisSanitizers = ibatis - ibatis.FindByShortName("execute*");

// All replaces that contain \r or \n as first parameter should be removed
CxList replace = Find_Methods().FindByShortName("replace*");
CxList replaceEnter = Find_Strings().GetParameters(replace, 0);
replaceEnter = replaceEnter.FindByShortName(@"*[^a-zA-Z]*");
replace = replace.FindByParameters(replaceEnter);
CxList exec = 
	All.FindByMemberAccess("Runtime.exec") + 
	All.FindByMemberAccess("getRuntime.exec") +
	All.FindByMemberAccess("System.exec") +
	All.FindByMemberAccess("Executor.safeExec");

CxList setStatus = All.FindByMemberAccess("HttpServletResponse.setStatus");

CxList filter = All.FindByMemberAccess("ResponseUtils.filter");

/* GWT Sanitizer */
CxList GWTSanitizer = All.FindByMemberAccess("SafeHtml.asString");
/* GWT */

// esapi
CxList getESAPI = Get_ESAPI();
CxList esAPISanitize = getESAPI.FindByMemberAccess("Encoder.encodeForJavaScript") +
	getESAPI.FindByMemberAccess("Encoder.encodeForVBScript") +
	getESAPI.FindByMemberAccess("Encoder.encodeForCSS");
// esapi
	
result = Find_XSS_Replace() +  
	Find_Encode() + 
	esAPISanitize +
	Find_General_Sanitize() +
	jspSanitizers +
	escapeSanitizers +
	GWTSanitizer +
	jspCode.GetParameters(jspSanitizers) +
	Find_Replace_Param() +
	ibatisSanitizers +
	Find_Parameters() +
	exec +
	replace +
	setStatus +
	filter;

result -= result.FindByType(typeof(ClassDecl));
result -= result.FindByType(typeof(TypeDecl));
result -= result.FindByType(typeof(MethodDecl));
result -= result.FindByType(typeof(NamespaceDecl));
result -= Find_Strings();