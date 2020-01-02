CxList methodDecl = Find_MethodDeclaration();

CxList requestGet = All.FindByMemberAccess("request.get*");

CxList inputs = All.NewCxList();
inputs.Add(requestGet.FindByMemberAccess("request.getCharacterEncoding"));
inputs.Add(requestGet.FindByMemberAccess("request.getContentType"));
inputs.Add(All.FindByMemberAccess("request.ContentType"));
inputs.Add(requestGet.FindByMemberAccess("request.getInputStream"));
inputs.Add(All.GetParameters(requestGet.FindByMemberAccess("request.getParameter"), 0));
inputs.Add(requestGet.FindByMemberAccess("request.getParameterValues"));
inputs.Add(All.FindByMemberAccess("request.ParameterValues"));
inputs.Add(requestGet.FindByMemberAccess("request.getReader"));
inputs.Add(requestGet.FindByMemberAccess("request.getParameterNames"));
inputs.Add(All.FindByMemberAccess("request.ParameterNames"));
inputs.Add(requestGet.FindByMemberAccess("request.getParameterMap"));				
inputs.Add(All.FindByMemberAccess("request.ParameterMap"));		
inputs.Add(requestGet.FindByMemberAccess("request.getHeader"));
inputs.Add(requestGet.FindByMemberAccess("request.getHeaders"));
inputs.Add(requestGet.FindByMemberAccess("request.getHeaderNames"));
inputs.Add(requestGet.FindByMemberAccess("request.getQueryString"));
inputs.Add(requestGet.FindByMemberAccess("request.getRequestedSessionId"));
inputs.Add(requestGet.FindByMemberAccess("request.getPathInfo"));
inputs.Add(requestGet.FindByMemberAccess("request.getRemoteUser"));
inputs.Add(requestGet.FindByMemberAccess("request.getRequestURI"));
inputs.Add(requestGet.FindByMemberAccess("request.getRequestURL"));
inputs.Add(All.FindByMemberAccess("MultipartHttpServletRequest.getFile"));
inputs.Add(All.FindByMemberAccess("RequestContext.get*"));
inputs.Add(All.FindByMemberAccess("Text.getText"));
inputs.Add(All.FindByMemberAccess("TextComponent.getText"));
inputs.Add(All.FindByMemberAccess("Socket.getInputStream"));
inputs.Add(All.FindByMemberAccess("JTextComponent.get*"));
inputs.Add(All.FindByMemberAccess("TextArea.getText")); 
inputs.Add(All.FindByMemberAccess("TextField.getText"));
inputs.Add(All.FindByMemberAccess("wmgetRequestedSessionId.getRequestedSessionId*"));
inputs.Add(All.FindByMemberAccess("WebSession.getRequest"));
inputs.Add(All.FindByMemberAccess("HttpServletRequestWrapper.getParameter*"));
inputs.Add(All.FindByMemberAccess("ServletRequestWrapper.getParameter*"));
inputs.Add(All.FindByMemberAccess("HttpServletRequest.getParameter*"));

CxList console = All.FindByMemberAccess("Console.*");

inputs.Add(console.FindByShortName("readPassword"));
inputs.Add(console.FindByShortName("readLine"));

CxList scanner = All.FindByMemberAccess("Scanner.*");

inputs.Add(scanner.FindByShortName("next"));
inputs.Add(scanner.FindByShortName("nextBigDecimal"));
inputs.Add(scanner.FindByShortName("nextBigInteger"));
inputs.Add(scanner.FindByShortName("nextBoolean"));
inputs.Add(scanner.FindByShortName("nextByte"));
inputs.Add(scanner.FindByShortName("nextDouble"));
inputs.Add(scanner.FindByShortName("nextFloat"));
inputs.Add(scanner.FindByShortName("nextInt"));
inputs.Add(scanner.FindByShortName("nextLine"));
inputs.Add(scanner.FindByShortName("nextLong")); 	
inputs.Add(scanner.FindByShortName("nextShort")); 

CxList cookiesInput = 
	All.FindByMemberAccess("Cookie.get*") + 
	All.FindByMemberAccess("request.getCookies") +
	All.FindByMemberAccess("request.cookies");
CxList webSessionMethods = All.FindByMemberAccess("WebSession.getRequest");
cookiesInput.Add(webSessionMethods.GetMembersOfTarget().FindByShortName("getCookies"));


inputs.Add(cookiesInput);

CxList ThisBaseMembers = (All.FindByType(typeof(ThisRef)) + 
	All.FindByType(typeof(BaseRef))).GetMembersOfTarget();

inputs = inputs - ThisBaseMembers;

CxList methods = Find_Methods();
inputs.Add(methods.FindByShortName("getRawParameter"));

inputs.Add(All.GetParameters(methodDecl.FindByName("*.main")
	.FindByFieldAttributes(Modifiers.Public | Modifiers.Static)));

if(!All.isWebApplication){
	CxList inCommand = All.FindByName("System.in.*");
	CxList parameters = All.GetParameters(inCommand, 0);
	inputs.Add(parameters + All.FindByName("System.in") + 
		All.FindByMemberAccess("URLConnection.getInputStream"));
}

inputs.Add(All.FindByMemberAccess("SwingInputField.getText") + 
	All.FindByMemberAccess("HyperlinkEvent.getURL"));

CxList socket = All.FindByMemberAccess("socket.get*");
CxList inputStreamReader = All.FindByShortName("InputStreamReader").GetAncOfType(typeof(ObjectCreateExpr));
CxList In = All.FindAllReferences(All.FindByType("BufferedReader"));
In = In.DataInfluencedBy(socket + inputStreamReader);
In = In.GetMembersOfTarget();
CxList bufferReader_read = In.FindByMemberAccess("BufferedReader.read*");
inputs -= inputs.GetByAncs(inputStreamReader).DataInfluencingOn(bufferReader_read);

inputs.Add(bufferReader_read);

// properties
CxList properties = All.FindByMemberAccess("Properties.load") +
	All.FindByMemberAccess("Properties.get");

//scala
CxList readMethods = methods.FindByShortName("read*");
CxList scalaInputMethods = readMethods.FindByShortName("readLine");
scalaInputMethods.Add(readMethods.FindByShortName("readBoolean"));
scalaInputMethods.Add(readMethods.FindByShortName("readDouble"));
scalaInputMethods.Add(readMethods.FindByShortName("readFloat"));
scalaInputMethods.Add(readMethods.FindByShortName("readInt"));
scalaInputMethods.Add(readMethods.FindByShortName("readShort"));
scalaInputMethods.Add(readMethods.FindByShortName("readf"));
scalaInputMethods.Add(readMethods.FindByShortName("readf1"));
scalaInputMethods.Add(readMethods.FindByShortName("readf2"));
scalaInputMethods.Add(readMethods.FindByShortName("readf3"));
//remove stuff like BufferedReader.readline()
scalaInputMethods -= scalaInputMethods.GetTargetOfMembers().GetMembersOfTarget();

CxList scalaSwingInputs = All.FindByMemberAccess("TextArea.text");
scalaSwingInputs.Add(All.FindByMemberAccess("TextComponent.text"));
scalaSwingInputs.Add(All.FindByMemberAccess("TextField.text"));

//Akka Inputs
CxList classes = Find_Class_Decl();
//Fetch classes that inherit from class RequestBuilding
classes = classes.InheritsFrom("RequestBuilding");
//Get methods that are defined in class RequestBuilding
CxList methodsInherit = methods.GetByAncs(classes);
//Inputs
CxList akkaInputs = methodsInherit.FindByShortNames(new List<String>{"Delete", "Get", "Head", "Options", "Patch", "Post", "Put"});

result = inputs;
result.Add(properties);
result.Add(Find_WebServices_Input());
result.Add(scalaInputMethods);
result.Add(scalaSwingInputs);
result.Add(akkaInputs);
result -= Not_Interactive_Inputs();