CxList methodDecl = Find_MethodDecls();
CxList mainInput = All.GetParameters(methodDecl.FindByShortName("main"));

CxList methods = Find_Methods();
//Workaround since there's no MemberAccess instances.
CxList nonMemberFunctionCalls = methods - methods.GetMembersWithTargets();
CxList userInput = nonMemberFunctionCalls.FindByShortName("readLine");

CxList requests = All.FindByType("HttpServletRequest");

CxList requestGet = requests.GetMembersOfTarget();
List <string> gets = new List<string> {
		"getCharacterEncoding",
		"getContentType",
		"ContentType",
		"getInputStream",
		"getParameter",
		"getParameterValues",
		"ParameterValues",
		"getReader",
		"getReader",
		"getParameterMap",
		"ParameterMap",
		"getHeader",
		"getHeaders",
		"getHeaderNames",
		"getQueryString",
		"getRequestedSessionId",
		"getPathInfo",
		"getRemoteUser",
		"getRequestURI",
		"getRequestURL",
		"getCookies",
		"cookies"
		};
CxList inputs = requestGet.FindByShortNames(gets);

inputs.Add(All.FindByMemberAccess("MultipartHttpServletRequest.getFile"));
inputs.Add(All.FindByMemberAccess("RequestContext.get*"));
inputs.Add(All.FindByMemberAccess("Socket.getInputStream"));
inputs.Add(All.FindByMemberAccess("wmgetRequestedSessionId.getRequestedSessionId*"));
inputs.Add(All.FindByMemberAccess("WebSession.getRequest"));

inputs.Add(All.FindByMemberAccess("EditText.getText"));

CxList console = All.FindByMemberAccess("Console.*");
inputs.Add(console.FindByShortName("readPassword"));
inputs.Add(console.FindByShortName("readLine"));

inputs.Add(All.FindByMemberAccess("Scanner.next*"));
inputs.Add(All.FindByMemberAccess("Scanner.find*"));
inputs.Add(All.FindByMemberAccess("Scanner.tokens"));

CxList cookieValueAnnotation = Find_CustomAttribute().FindByCustomAttribute("CookieValue");
inputs.Add(cookieValueAnnotation.GetFathers());
inputs.Add(All.FindByMemberAccess("Cookie.get*"));
inputs.Add(All.FindByMemberAccess("Cookie.value"));
inputs.Add(All.FindByMemberAccess("Cookie.attributes"));
inputs.Add(All.FindByMemberAccess("Cookie.valueWithAttributes"));
inputs.Add(All.FindByMemberAccess("Cookie.key"));

CxList urlInputs = All.FindByMemberAccess("URLConnection.getInputStream");
urlInputs.Add(All.FindByMemberAccess("URLConnection.inputStream"));
urlInputs.Add(methods.FindByMemberAccess("URL.readText"));
inputs.Add(urlInputs);

inputs.Add(All.FindByMemberAccess("Socket.getInputStream"));

result = mainInput;
result.Add(userInput);
result.Add(inputs);

// If it is an Android project - Add Android interactive inputs
if(Find_Android_Settings().Count > 0)
{
	result.Add(Find_Android_Interactive_Inputs());
}