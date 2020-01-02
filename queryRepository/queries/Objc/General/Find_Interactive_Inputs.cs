CxList cin = All.FindByShortName("cin");
CxList exp = cin.GetAncOfType(typeof(BinaryExpr));
CxList methodInvoke = Find_Methods();
CxList methodDecl = Find_MethodDecls();
string[] mainDeclNames = {"main", "Main", "_main", "_tmain", "Winmain", "AfxWinMain"};
CxList main = methodDecl.FindByShortNames(new List<string>(mainDeclNames));

// Add the other inputs
string[] interactiveInputsNames = {"getchar", "getc", "getch", "getche", "kbhit", "getenv", "getenv_s",
	"_wgetenv",	"_wgetenv_s", "catgets", "getdlgtext", "getpass"};
CxList interactiveInputs = methodInvoke.FindByShortNames(new List<string>(interactiveInputsNames));

interactiveInputs.Add(All.GetByAncs(All.GetParameters(methodInvoke.FindByShortName("scanf")) -
	All.GetParameters(methodInvoke.FindByShortName("scanf"), 0)));
interactiveInputs.Add(All.GetParameters(main));
interactiveInputs.Add(All.FindByShortName("m_lpCmdLine"));
interactiveInputs.Add(All.GetParameters(methodInvoke.FindByShortName("gets"), 0)); //the first param is input
interactiveInputs.Add(methodInvoke.FindByMemberAccess("NSProcessInfo.processInfo"));
interactiveInputs.Add(methodInvoke.FindByMemberAccess("ProcessInfo.processInfo"));
interactiveInputs.Add(All.FindByFathers(exp));
interactiveInputs -= cin;

result = interactiveInputs;

// Go over the functions under UIApplicationDelegate
CxList applicationBackground = All.InheritsFrom(@"UIApplicationDelegate");
CxList didFinishLaunchingWithOptions = methodDecl.FindByShortName(@"application:didFinishLaunchingWithOptions:").GetByAncs(applicationBackground);
CxList uiApplicationDelegateParameters = All.GetParameters(didFinishLaunchingWithOptions, 1);

CxList didReceiveRemoteNotification = methodDecl.FindByShortName(@"application:didReceiveRemoteNotification:").GetByAncs(applicationBackground);
result.Add(All.GetParameters(didReceiveRemoteNotification, 1));

CxList userActivity = methodDecl.FindByShortName(@"application:didUpdateUserActivity:").GetByAncs(applicationBackground);
userActivity.Add(methodDecl.FindByShortName(@"application:continueUserActivity:restorationHandler:").GetByAncs(applicationBackground));
// NSUserActivity is an input, but there is no flow from the parameter declarators of userActivity objects to their references,
//	and so add their references to inputs, as well as their parameter declaration reference
CxList userActivityParam = All.GetParameters(userActivity, 1);
CxList userActivityRef = All.FindAllReferences(userActivityParam);
result.Add(userActivityRef);

CxList objectForKeyMethods = All.FindAllReferences(uiApplicationDelegateParameters)
	.GetMembersOfTarget().FindByShortName("objectForKey:");

CxList objectForKeyParams = All.GetParameters(objectForKeyMethods)
	.FindByShortName("UIApplicationLaunchOptionsURLKey").FindByType(typeof(Param));

CxList objectForKeyRelevantCalls = All.FindByParameters(objectForKeyParams);
CxList startPaths = objectForKeyRelevantCalls.DataInfluencedBy(uiApplicationDelegateParameters);

CxList impacts = All.DataInfluencedBy(startPaths);
// Get Direct impact of input parameters
CxList endPaths = impacts.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllButNotStartAndEnd)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
CxList directInputImpact = endPaths.DataInfluencedBy(uiApplicationDelegateParameters).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

result.Add(directInputImpact);

CxList NSUserDefaults = All.FindByMemberAccess("NSUserDefaults.*");

List<string> nsUserDefaultsMethodsNames = new List<string> {
		"stringForKey:","arrayForKey:","boolForKey:","dataForKey:",
		"dictionaryForKey:","floatForKey:","integerForKey:",
		"objectForKey:","stringArrayForKey:","doubleForKey:","URLForKey:"
		};

CxList NSUserDefaultsInput = NSUserDefaults.FindByShortNames(nsUserDefaultsMethodsNames);

//swift
CxList UserDefaults = All.FindByMemberAccess("UserDefaults.*");

List<string> userDefaultsMethodsNames = new List<string> {
		"stringForKey:","array:","bool:","data:","dictionary:",
		"float:","integer:","object:","stringArray:","double:", "url:"
		};

CxList UserDefaultsInput = UserDefaults.FindByShortNames(userDefaultsMethodsNames);

result.Add(NSUserDefaultsInput, UserDefaultsInput);

CxList allParameters = All.GetParameters(methodInvoke.FindByMemberAccess("CEdit.Get*"), 1);
allParameters.Add(All.GetParameters(methodInvoke.FindByMemberAccess("CRichEditCtrl.Get*"), 1));
allParameters.Add(All.GetParameters(methodInvoke.FindByMemberAccess("CComboBox.Get*"), 1));
allParameters.Add(All.GetParameters(methodInvoke.FindByShortName("GetWindowText*"), 1));

result.Add(All.GetByAncs(allParameters));

result.Add(cin.FindByType(typeof(UnknownReference)));

result.Add(All.FindByShortName("stdin"));

// Add stream methods
CxList inGet = methodInvoke.FindByMemberAccess("istream.get");
CxList inGetline = methodInvoke.FindByMemberAccess("istream.getline");


CxList inGetlineAll = All.NewCxList();
inGetlineAll.Add(inGet, inGetline);

inGet.Add(All.GetByAncs(All.GetParameters(inGetlineAll, 0)));

CxList inPeek = methodInvoke.FindByMemberAccess("istream.peek");

CxList inRead = methodInvoke.FindByMemberAccess("istream.read*");
inRead = All.GetByAncs(All.GetParameters(inRead, 0));

CxList inPutBack = methodInvoke.FindByMemberAccess("istream.putback");
inPutBack = All.GetByAncs(All.GetParameters(inRead, 0));

CxList inSbumpc = All.GetByAncs(All.GetParameters(methodInvoke.FindByMemberAccess("streambuf.sbumpc"), 0));
CxList inSgetc = methodInvoke.FindByMemberAccess("streambuf.sgetc");
CxList inSgetn = All.GetByAncs(All.GetParameters(methodInvoke.FindByMemberAccess("streambuf.sgetn"), 0));
CxList inSnextc = methodInvoke.FindByMemberAccess("streambuf.snextc");
CxList inSputbackc = All.GetByAncs(All.GetParameters(methodInvoke.FindByMemberAccess("streambuf.sputbackc"), 0));

result.Add(inGet ,inPeek ,inRead ,inPutBack ,inSbumpc ,inSgetc ,inSgetn ,inSnextc ,inSputbackc);

// Add the SendMessage/PostMessage things when type is WM_GETTEXT
string[] sendMessageNames = {"SendMessage", "SendMessageCallback", "SendNotifyMessage", "PostMessage", "PostThreadMessage"};
CxList sendMessage = methodInvoke.FindByShortNames(new List<string>(sendMessageNames));

CxList sendMessageParams = All.GetParameters(sendMessage);
CxList sendMessageSet = sendMessageParams.FindByType("WM_GETTEXT");
sendMessage = sendMessage.FindByParameters(sendMessageSet);

result.Add(sendMessageParams.GetParameters(sendMessage, 2));
result.Add(sendMessageParams.GetParameters(sendMessage, 3));

// Filter:
result -= result.FindByType(typeof(Param)); // Remove the Param itself from the parameters. It is not needed. 
result -= result.FindByType(typeof(UnaryExpr)); // Remove pointers and addresses characters (&, *)

result.Add(Find_Interactive_Inputs_User());
result.Add(Find_Interactive_Inputs_Web());


CxList allOfNSEXType = All.FindByType("NSExtensionContext").GetMembersOfTarget().FindByShortName("inputItems");
CxList directAccess = All.FindByShortName("extensionContext").GetRightmostMember();
result.Add(allOfNSEXType, directAccess);
// Find and remove input from the main bundle, which is not an input and is safe:
CxList NSbundle = All.FindByTypes(new String[]{"NSBundle","Bundle"});
CxList mainBundleObject = methodInvoke.FindByMemberAccess("NSBundle.mainBundle"); // [NSBundle mainBundle]
mainBundleObject.Add(methodInvoke.FindByMemberAccess("Bundle.main"));

// find pattern:
//				NSBundle* ptr;
// 				ptr = [NSBundle mainBundle];
CxList mainBundlePtr = NSbundle.FindByAssignmentSide(CxList.AssignmentSide.Left).FindByFathers(mainBundleObject.GetFathers()); 

// find pattern: NSBundle* ptr = [NSBundle mainBundle];
CxList mainBundleRef = NSbundle.FindAllReferences(mainBundlePtr);
mainBundleRef.Add(mainBundleObject);

/* static methods of NSBundle class are not safe:
	"URLForResource:withExtension:subdirectory:inBundleWithURL:",
	"URLsForResourcesWithExtension:subdirectory:inBundleWithURL:"
	"pathsForResourcesOfType:inDirectory:",
	"pathForResource:ofType:inDirectory:",*/

// instance methods for NSBundle objects 
string[] mainBundleMethodsNames = {"URLForResource:withExtension:subdirectory:", "URLForResource:withExtension:",
	"pathForResource:ofType:", "URLsForResourcesWithExtension:subdirectory:",
	"pathForResource:ofType:inDirectory:", "URLForResource:withExtension:subdirectory:localization:",
	"pathForResource:ofType:inDirectory:forLocalization:", "pathsForResourcesOfType:inDirectory:",
	"URLsForResourcesWithExtension:subdirectory:localization:", "pathsForResourcesOfType:inDirectory:forLocalization:",
	"resourcePath", "resourceURL"
	};

CxList NSBundleMethods = methodInvoke.FindByShortNames(new List<string>(mainBundleMethodsNames));

// Swift
List<string> mainBundleMethodsNamesSwift = new List<string> {		
		"url:forResource:withExtension:subdirectory:",
		"url:forResource:withExtension:",
		"url:forResource:withExtension:subdirectory:localization:",
		"urls:forResourcesWithExtension:subdirectory:",
		"urls:forResourcesWithExtension:subdirectory:localization:",
		"path:forResource:ofType:", 	
		"path:forResource:ofType:inDirectory:", 
		"path:forResource:ofType:inDirectory:forLocalization:",
		"paths:forResourcesOfType:inDirectory:",	
		"paths:forResourcesOfType:inDirectory:forLocalization:",
		"resourcePath",
		"resourceURL"
		};

NSBundleMethods.Add(methodInvoke.FindByShortNames(mainBundleMethodsNamesSwift));

List<string> mainBundleMethodsNamesSecondColl = new List<string> {
		"url:withExtension:subdirectory:",
		"url:withExtension:",
		"url:withExtension:subdirectory:localization:",
		"urls:subdirectory:",
		"urls:subdirectory:localization:",
		"path:ofType:", 	
		"path:ofType:inDirectory:", 
		"path:ofType:inDirectory:forLocalization:",
		"paths:inDirectory:",	
		"paths:inDirectory:forLocalization:"
		};


CxList mainBundleMethodsNamesSecondCollAll = methodInvoke.FindByShortNames(mainBundleMethodsNamesSecondColl);
	
NSBundleMethods.Add(mainBundleMethodsNamesSecondCollAll.FindByParameterName("forResource", 0));
NSBundleMethods.Add(mainBundleMethodsNamesSecondCollAll.FindByParameterName("forResourcesOfType", 0));
NSBundleMethods.Add(mainBundleMethodsNamesSecondCollAll.FindByParameterName("forResourcesWithExtension", 0));

CxList mainBundleMethods = NSBundleMethods * mainBundleRef.GetMembersOfTarget();

CxList objAssigned = All.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList objAssignedSafe = objAssigned.FindByFathers(mainBundleMethods.GetFathers()); // for example: NSString* str; str = [[NSBundle mainBundle] method];
objAssignedSafe.Add(Find_Declarators() * mainBundleMethods.GetFathers()); // for example: NSString* str = [[NSBundle mainBundle] method];

CxList objAssignedSafeBundleMethods = All.NewCxList();
objAssignedSafeBundleMethods.Add(objAssignedSafe, mainBundleMethods);

result -= methodInvoke.FindByParameters(All.FindAllReferences(objAssignedSafeBundleMethods)); // remove methods that recieve a parameter which origins from the main bundle

// Add relevant application(..) parameters
CxList application = methodDecl.FindByShortName("application:*");
CxList applicationParameters = All.GetParameters(application);
CxList applicationParametersTypes = Find_TypeRef().GetByAncs(applicationParameters);
CxList relevantTypeRefs = applicationParametersTypes.FindByShortNames(
	new List<string> {"NSDictionary", "NSURL", "NSUserActivity"});
CxList relevantApplicationParameters = relevantTypeRefs.GetAncOfType(typeof(ParamDecl));
result.Add(All.FindAllReferences(relevantApplicationParameters));