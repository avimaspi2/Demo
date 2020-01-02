// All the methods in the list return a environment variable that is a input
CxList methodInvoke = Find_Methods();
CxList getenvMethods = methodInvoke.FindByShortNames(
	new List<string>{"getenv","getenv_s","_wgetenv","_wgetenv","_wgetenv_s"});

// When using library cgicc (https://www.gnu.org/software/cgicc/) the next methods return environment variable QUERY_STRING
CxList cgicc = All.FindByType("Cgicc");
CxList cgiccMethods = cgicc.GetMembersOfTarget().FindByShortNames(
	new List<string>{"getElement","getElementByValue","operator","getElements","getFile","getFiles"});

/* It is possible to access to cgiEnvironement and then to environment variables, for instance:
		const CgiEnvironment& env = cgi.getEnvironment();
		string = env.getQueryString();	*/
CxList environmentElems = cgicc.GetMembersOfTarget().FindByShortName("getEnvironment");
CxList unknownRefs = Find_Unknown_References();

environmentElems.Add(unknownRefs.FindAllReferences(environmentElems.GetAssignee()));

CxList environmentVars = environmentElems.GetMembersOfTarget().FindByShortNames(
	new List<string>{"getAccept","getUserAgent","getRedirectRequest","getRedirectURL","getCookies","getCookieList",
		"getPathInfo","getContentLength","getContentType","getPostData"});

result = getenvMethods;
result.Add(cgiccMethods);
result.Add(environmentVars);