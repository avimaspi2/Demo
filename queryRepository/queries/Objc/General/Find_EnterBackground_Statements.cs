char[] trimChars = new char[6] {' ', '\t', '"', '(', '\r', '\n'};

CxList methods = Find_Methods();
CxList allParams = Find_Param();
CxList methDecl = Find_MethodDecls();
CxList commands = Find_Statements();
// Add the function under UIApplicationDelegate
CxList applicationBackground = All.InheritsFrom(@"UIApplicationDelegate");
applicationBackground = methDecl.FindByShortName(@"applicationDidEnterBackground:")
	.GetByAncs(applicationBackground);

// Find the functions that run right before entering background.
CxList enterBackground = methods.FindByShortName("addObserver:selector:name:object:");
enterBackground = All.FindByShortName("UIApplicationDidEnterBackgroundNotification").GetParameters(enterBackground, 2);
enterBackground = enterBackground.GetAncOfType(typeof(MethodInvokeExpr));

enterBackground = methods.FindByShortName("@selector").GetParameters(enterBackground, 1);
CxList backGroundMeth = allParams.GetParameters(enterBackground);

CxList res = All.NewCxList();
foreach (CxList enter in backGroundMeth)
{
	CSharpGraph gr = enter.TryGetCSharpGraph<CSharpGraph>();
	string name = gr.ShortName.TrimStart(trimChars);
	res.Add(methDecl.FindByShortName(name));	
}

CxList methodsInCallBack = commands.GetByAncs(res);
CxList inheritsNSAppDelegate = All.InheritsFrom("UIApplicationDelegate");
CxList didBackGround = methDecl.GetByAncs(inheritsNSAppDelegate).FindByShortName("applicationDidEnterBackground:");
CxList methodsinDidBackGround = commands.GetByAncs(didBackGround);
/*
enterBackground = Find_Strings().GetParameters(enterBackground, 0);// +postNotifications;
enterBackground = All.FindByType(typeof(MethodDecl)).FindByShortName(enterBackground);
*/
result = methodsInCallBack;
result.Add(methodsinDidBackGround);