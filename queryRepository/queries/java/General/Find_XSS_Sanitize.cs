CxList jspCode = Find_Jsp_Code();
CxList jspTags = Find_Output_Tags();
CxList strings = Find_Strings();
CxList methods = Find_Methods();
methods.Add(base.Find_MethodRef());

CxList escapeSanitizers = methods.FindByShortNames(new List<string> {
		"escapeXml*",
		"escapeJavaScript*",
		"escapeHtml*",
		"escapeEcmaScript*",
		"htmlEscape*",
		"getEscapeXml*",
		"getEscapeJavaScript*",
		"getEscapeHtml*",
		"getHtmlEscape*",
		"addEntities*"});

CxList jspSanitizers = jspCode.GetByAncs(jspTags);

// Methods from SpringMVC that are not outputs for XSS
CxList springMvcSanitizers = methods.FindByShortNames(new List<string> {
		"isRequestedSessionIdFromCookie",
		"isRequestedSessionIdFromUrl",
		"isRequestedSessionIdFromURL",
		"isRequestedSessionIdValid",
		"isUserInRole",
		"authenticate",
		"getIntHeader",
		"getDateHeader"});

CxList mvcSanitizersValues = jspCode.FindByMemberAccess("c_out.value*");
mvcSanitizersValues.Add(jspCode.FindByMemberAccess("c_param.value"));

CxList mvcSanitizersGetMembersOfTarget = mvcSanitizersValues.GetMembersOfTarget();

CxList xPathSanitize = All.FindByMemberAccess("XPATH.evaluate");

CxList mvcSanitizers = mvcSanitizersValues.FindByMemberAccess("response.write");
mvcSanitizers.Add(mvcSanitizersGetMembersOfTarget.FindByMemberAccess("response.write"));
mvcSanitizers.Add(mvcSanitizersGetMembersOfTarget.GetMembersOfTarget().FindByMemberAccess("response.write"));

CxList beanWriteSanitizers = jspCode.FindByMemberAccess("bean_write.response").GetMembersOfTarget().FindByMemberAccess("response.write");

CxList mvcBeans = All.NewCxList();
mvcBeans.Add(mvcSanitizers, beanWriteSanitizers);

jspSanitizers.Add(jspCode.GetByAncs(mvcBeans));

CxList jspEscaped = jspCode.FindByMemberAccess("cx_escFalse.*");
jspSanitizers -= jspCode.GetByAncs(jspEscaped.GetFathers().GetAncOfType(typeof(MethodInvokeExpr)) * beanWriteSanitizers);
jspSanitizers -= jspCode.GetByAncs(jspEscaped.GetAncOfType(typeof(MethodInvokeExpr)));

CxList methodsInJspSanitizers = jspSanitizers.FindByType(typeof(MethodRef));
methodsInJspSanitizers.Add(jspSanitizers.FindByType(typeof(MethodInvokeExpr)));

CxList gettersInJspSanitizers = methodsInJspSanitizers.FindByShortName("get*");
jspSanitizers -= gettersInJspSanitizers.GetByAncs(jspSanitizers.FindByAssignmentSide(CxList.AssignmentSide.Right));

/************ JSF sanitizers *******************/
CxList jsfCode = Find_JSF_Code();
CxList jsfSanitizers = jsfCode.FindByMemberAccess("response.write"); // ny default in JSF it is sanitized

// remove the ones that were set escape="false"
CxList jsfNotEscaped = jsfCode.FindByMemberAccess("cx_escFalseJSF.*");
CxList jsfNotSanitizers = jsfCode.GetByAncs(jsfNotEscaped.GetFathers().GetAncOfType(typeof(MethodInvokeExpr)));

jsfSanitizers -= jsfNotSanitizers;

/************ END JSF sanitizers *******************/

CxList ibatis = Ibatis();
CxList ibatisSanitizers = ibatis - ibatis.FindByShortName("execute*");

// add time manipulation as a sanitizer 
CxList timeManipulation = methods.FindByMemberAccess("SimpleDateFormat.parse");
timeManipulation.Add(methods.FindByMemberAccess("GregorianCalendar.setTime")); 
timeManipulation.Add(methods.FindByMemberAccess("TimeZone.getTimeZone"));

// All replaces that contain \r or \n as first parameter should be removed
CxList replace = methods.FindByShortName("replace*");
CxList replaceEnter = strings.GetParameters(replace, 0);
replaceEnter = replaceEnter.FindByShortName(@"*[^a-zA-Z]*");
replace = replace.FindByParameters(replaceEnter);

CxList exec = All.FindByMemberAccess("Runtime.exec");
exec.Add(All.FindByMemberAccess("getRuntime.exec"));
exec.Add(All.FindByMemberAccess("System.exec"));
exec.Add(All.FindByMemberAccess("Executor.safeExec"));

CxList setStatus = All.FindByMemberAccess("HttpServletResponse.setStatus");

CxList filter = All.FindByMemberAccess("ResponseUtils.filter");

/* GWT Sanitizer */
CxList SimpleHtmlSanitizerMethods = All.FindByMemberAccess("SimpleHtmlSanitizer.getInstance");
CxList GWTsanitizeMthd = All.FindByMemberAccess("sanitize.asString");
CxList GWTSanitizer = All.FindByMemberAccess("SafeHtml.asString");
GWTSanitizer.Add(All.FindByMemberAccess("sanitizeHtml.asString"));
GWTSanitizer.Add(SimpleHtmlSanitizerMethods.GetRightmostMember() * GWTsanitizeMthd);
/* GWT */

//general encoders
CxList encoders = Find_Encode();
// Remove sanitation if in a JS event, unless encoding for JS.
encoders -= encoders.FindByRegex(@"<[^>]+(onclick|ondblclick|onmousedown|onmousemove|onmouseover|onmouseout|onmouseup|onchange|oncontextmenu|oncopy|oncut|onerror|onfocus|onkeydown|onkeypress|onkeyup|onload|onpaste|onreset|onresize|onscroll|onsubmit)\s*=[^>]*for(?!JavaScript)[^>]*>");

CxList jsfFrameworkOutputs = methods.FindByShortName("CxJsfOutput");
CxList jsfVulnerable = methods.FindByShortName("CxJsfEscapeFalse");
jsfVulnerable = jsfFrameworkOutputs.GetByAncs(jsfVulnerable);
jsfFrameworkOutputs -= jsfVulnerable;

//RESTful
//This part looking for classes with Custom Atribute "@Produces(MediaType.APPLICATION_JSON)". All Methods of such class is protected from XSS 

CxList all_methodsDecls = Find_MethodDeclaration();
CxList all_paramDecl = Find_ParamDeclaration();
CxList allClasses = Find_Class_Decl();

CxList customAttributeProduces = Find_CustomAttribute().FindByCustomAttribute("Produces");
CxList producesJSON = customAttributeProduces.FindByRegex(@"MediaType\.APPLICATION_JSON");
producesJSON.Add(customAttributeProduces.FindByRegex(@"application/json"));
CxList fatherOfCs = producesJSON.GetFathers();

CxList goodMethods = all_methodsDecls * fatherOfCs;
CxList goodClasses = allClasses * fatherOfCs;
goodMethods.Add(all_methodsDecls.GetByAncs(goodClasses));
CxList MethodOfClsWithAPPLICATION_JSON = all_paramDecl.GetParameters(goodMethods);
//RESTful                

// Finds usage of @RequestMapping(..., produces = {"application/json"}), because 
// those generate json data. We then remove from the outputs the return results of 
// methods annotated with that item
CxList reqMappings = Find_CustomAttribute().FindByCustomAttribute("RequestMapping");
CxList appJsonAssignee = Find_Strings().FindByShortName(@"application/json").GetFathers().GetAssignee();
CxList producesParams = appJsonAssignee.FindByShortName("produces");
reqMappings = reqMappings * producesParams.GetFathers().GetFathers();
CxList retStmts = Find_ReturnStmt().GetByAncs(reqMappings.GetFathers());
CxList retChilds = All.FindByFathers(retStmts);

// Sanitizes usage of new URI(...) and URI.create
CxList uriObjCreate = Find_Object_Create().FindByShortName("URI", true);
uriObjCreate.Add(methods.FindByMemberAccess("URI.create", true));

result = All.NewCxList();
result.Add(retChilds);
result.Add(uriObjCreate);
result.Add(MethodOfClsWithAPPLICATION_JSON);
result.Add(Find_XSS_Replace());

result.Add(timeManipulation);
result.Add(Find_General_Sanitize());
result.Add(springMvcSanitizers, jspSanitizers, jsfSanitizers, escapeSanitizers, GWTSanitizer);
result.Add(jspCode.GetParameters(jspSanitizers));
result.Add(Find_Replace_Param());

result.Add(ibatisSanitizers);
result.Add(Find_Parameters());
result.Add(exec);

result.Add(replace, setStatus, filter, encoders);

result.Add(jsfFrameworkOutputs, xPathSanitize);

// Response methods not prone to XSS 
CxList response = All.FindByMemberAccess("response.*");
result.Add(response.FindByShortNames(new List<string>{
	"setBufferSize",
	"setCharacterEncoding",
	"setContentType",
	"setHeader",
	"setLocale"}));
result.Add(All.FindByMemberAccess("ZipOutputStream.*"));

result -= result.FindByType(typeof(ClassDecl));
result -= result.FindByType(typeof(TypeDecl));
result -= result.FindByType(typeof(MethodDecl));
result -= result.FindByType(typeof(NamespaceDecl));
result -= strings;

result.Add(Find_Whitelisting());
result.Add(Find_AtgDspSanitize());
result.Add(Find_JSF_Sanitize());
result.Add(Find_Aliyun_securityUtil_Escapers());