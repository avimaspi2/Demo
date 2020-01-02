CxList methodInvokes = Find_Methods();	
CxList methodDecls = Find_Method_Declarations();
CxList unknownRefsAndMemberAccesses = Find_Unknown_References();
unknownRefsAndMemberAccesses.Add(Find_MemberAccesses());

//Standard Input
CxList cinRefs = unknownRefsAndMemberAccesses.FindByShortNames(new List<string> {"cin", "wcin"});
result.Add(cinRefs);

//Parameters of main functions
CxList mainMethodsParams = All.GetParameters(
	methodDecls.FindByShortNames(new List<string>{ "main", "Main", "_main", "_tmain", "Winmain", "AfxWinMain" })
	);
result.Add(mainMethodsParams);

//Common inputs
CxList inputMethods = methodInvokes.FindByShortNames(
	new List<string>{ "getchar", "getc", "getch", "getche", "kbhit", "getdlgtext", "getpass" }
	);
result.Add(inputMethods);
CxList methodsToFilter = Find_Unbounded_Methods();

//Added to remove the format string of C++
CxList methodsToFilterWithTwoParameters = methodsToFilter.FindByShortNames(new List<string>{ "scanf","vscanf"});
CxList methodsToFilterWithThreeParameters = methodsToFilter.FindByShortNames(new List<string>{ "vsscanf","vfscanf","fscanf"});
CxList unboundedMethodsParams = Find_Unknown_References().GetParameters(methodsToFilterWithTwoParameters,1);
unboundedMethodsParams.Add(Find_Unknown_References().GetParameters(methodsToFilterWithThreeParameters,0));
unboundedMethodsParams.Add(Find_Unknown_References().GetParameters(methodsToFilterWithThreeParameters, 2));
result.Add(unboundedMethodsParams);

result.Add(Find_Environment_Inputs());
result.Add(Find_Inputs_Sockets());
result.Add(Find_Inputs_MFC());

CxList otherInputs = All.FindByShortName("stdin") + All.FindByShortName("m_lpCmdLine");
result.Add(otherInputs);

// Stream methods
CxList inGet = methodInvokes.FindByMemberAccess("istream.get");
result.Add(inGet);

CxList inPeek = methodInvokes.FindByMemberAccess("istream.peek");
result.Add(inPeek);

CxList inSgetc = methodInvokes.FindByMemberAccess("streambuf.sgetc");
result.Add(inSgetc);

CxList inSnextc = methodInvokes.FindByMemberAccess("streambuf.snextc");
result.Add(inSnextc);

CxList streamMethods = inGet;
streamMethods.Add(methodInvokes.FindByMemberAccess("istream.getline"));
streamMethods.Add(methodInvokes.FindByMemberAccess("istream.read*"));
streamMethods.Add(methodInvokes.FindByMemberAccess("istream.putback"));
streamMethods.Add(methodInvokes.FindByMemberAccess("streambuf.sbumpc"));
streamMethods.Add(methodInvokes.FindByMemberAccess("streambuf.sgetn"));
streamMethods.Add(methodInvokes.FindByMemberAccess("streambuf.sputbackc"));
result.Add(All.GetByAncs(All.GetParameters(streamMethods, 0)));

// SendMessage/PostMessage when the type is WM_GETTEXT
CxList sendMessage = methodInvokes.FindByShortNames(new List<string> { 
		"SendMessage", "SendMessageCallback", "SendNotifyMessage", 
		"SendNotifyMessage", "PostMessage", "PostThreadMessage" 
		});
CxList sendMessageParams = All.GetParameters(sendMessage);
CxList sendMessageSet = sendMessageParams.FindByType("WM_GETTEXT");
sendMessage = sendMessage.FindByParameters(sendMessageSet);
result.Add(sendMessageParams.GetParameters(sendMessage, 2));
result.Add(sendMessageParams.GetParameters(sendMessage, 3));

// remove unwanted nodes
result -= result.FindByType(typeof(Param)); // Remove the Param itself from the parameters
result -= result.FindByType(typeof(UnaryExpr)); // Remove pointers and addresses characters (&, *)