CxList methods = Find_Methods();

List<string> methodsNames = new List<string> {
		"stringWithContentsOfURL:*","dataWithContentsOfURL:*",
		"dictionaryWithContentsOfURL:*","*WithContentsOfURL:*"
		};

CxList objCFromURL = methods.FindByShortNames(methodsNames);

List<string> relevantTypes = new List<string>() {"NSString*","String*", "NSData*", "NSDictionary*"};
CxList methodsTypes = methods.FindByShortNames(relevantTypes);

objCFromURL.Add(methodsTypes.FindByParameterName("contentsOfURL", 0));
objCFromURL.Add(methodsTypes.FindByParameterName("contentsOf", 0));
objCFromURL.Add(methodsTypes.FindByParameterName("object", 0));

CxList URLResponses = All.FindByType("*URLResponse") - Find_Declarators();

CxList cookies = All.FindByTypes(new String[]{"NSHTTPCookie","HTTPCookie"});
cookies = cookies * cookies.DataInfluencedBy(All.FindByTypes(new String[]{"NSHTTPCookieStorage","HTTPCookieStorage"}));

CxList methodDefs = Find_MethodDecls();

//Add NSURLConnection data received
CxList connection = All.NewCxList();

CxList connectionMethods = methodDefs.FindByShortName("connection:didReceiveData:");
connectionMethods.Add(methodDefs.FindByShortName("connection:didReceive:"));

connection = All.GetParameters(connectionMethods, 1);

//Input from URL scheme
CxList URLScheme = methodDefs.FindByShortName("application:handleOpenURL:*");	
URLScheme.Add(methodDefs.FindByShortName("application:openURL:*"));

URLScheme = All.GetParameters(URLScheme, 1);

result = objCFromURL;
result.Add(URLResponses);
result.Add(cookies);
result.Add(connection);
result.Add(URLScheme);

result -= result.FindByAssignmentSide(CxList.AssignmentSide.Left);