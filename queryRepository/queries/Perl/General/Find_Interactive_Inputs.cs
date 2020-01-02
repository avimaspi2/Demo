//Inputs
CxList methods = Find_Methods();
CxList strings = Find_Strings();
CxList unknown = All.FindByType(typeof(UnknownReference));

//Console Inputs
CxList streamAccess = All.GetParameters(methods.FindByShortName("<?>"));
streamAccess -= streamAccess.FindByType(typeof(Param));

CxList consoleInputs = streamAccess;
consoleInputs.Add(methods.FindByShortName("getc"));

//Command line Inputs
CxList commandLine = Find_Console_Inputs();

//Web Inputs
CxList queryStringParams = strings.FindByName("QUERY_STRING");
queryStringParams.Add(unknown.GetByAncs(queryStringParams.GetAncOfType(typeof(IndexerRef))));
CxList webInputs = methods.FindByShortName("param") + //parameters
	queryStringParams + //query string params
	methods.FindByMemberAccess("CGI", "param");

CxList unsafeEnvParams = 
	(strings.FindByShortName(@"HTTP_*") - strings.FindByShortName(@"HTTP_HOST") ) +
	strings.FindByShortName(@"REQUEST_URI");
webInputs.Add(unknown.GetByAncs(unsafeEnvParams.GetAncOfType(typeof(IndexerRef)).FindByShortName("*ENV")));

// Cookies
CxList cookies = 
	methods.FindByShortName("cookie") +
	methods.FindByMemberAccess("CGI::Cookie", "*fetch");

// sockets
//CxList sockets = All.GetParameters(methods.FindByShortName("socket"), 0);
CxList sockets = methods.FindByShortName("socket");

// Find inputs from request
CxList request = All.FindByType("*Request");
CxList eq = request.GetAncOfType(typeof(VariableDeclStmt));
request = All.GetByAncs(eq).FindByAssignmentSide(CxList.AssignmentSide.Left);
request = All.FindAllReferences(request);
request = request.GetMembersOfTarget().FindByShortName("parameters").GetMembersOfTarget();

// Add all to the result
result = consoleInputs + commandLine + webInputs + cookies + sockets + request;