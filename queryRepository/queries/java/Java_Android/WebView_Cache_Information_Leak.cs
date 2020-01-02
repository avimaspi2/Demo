CxList methods = Find_Methods();
CxList strings = Find_Strings();
CxList unknownRef = Find_UnknownReference();
CxList members = Find_MemberAccess();
CxList classes = Find_Class_Decl();

CxList urAndMembers = All.NewCxList();
urAndMembers.Add(unknownRef);
urAndMembers.Add(members);

// Look for deleting all cache files of the app
CxList WebViewDatabase = All.FindByType("WebViewDatabase");
CxList WebViewDatabaseInstance = members.FindByMemberAccess("WebViewDatabase.getInstance");
WebViewDatabase.Add(WebViewDatabase);
WebViewDatabase.Add(urAndMembers.FindAllReferences(WebViewDatabaseInstance.GetAssignee()));
CxList clearFormMethods = WebViewDatabase.GetMembersOfTarget().FindByShortName("clearFormData");

CxList clearApplicationUserData = methods.FindByMemberAccess("*.clearApplicationUserData");

CxList clearFormMethodsApplication = All.NewCxList();
clearFormMethodsApplication.Add(clearFormMethods);
clearFormMethodsApplication.Add(clearApplicationUserData);

if (clearFormMethodsApplication.Count == 0 )
{

	CxList webView = All.FindByType("WebView");
	webView.Add(urAndMembers.FindByType(classes.InheritsFrom("WebView")));
	CxList webViewMethods = webView.GetMembersOfTarget() * methods;

	CxList loginString = strings.FindByShortName("*login*");
	loginString.Add(strings.FindByShortName("*register*"));
	CxList loginPage = loginString.FindByShortNames(new List<string> {"*.htm", "*.html", "*.php", "*.asp"}, false);

	CxList loadURLMethods = webViewMethods.FindByShortName("loadUrl");
	CxList loadURLParameter = All.GetParameters(loadURLMethods, 0);
	CxList suspectParam = loadURLParameter.FindByAbstractValues(loginPage);
	CxList suspectMethods = loadURLMethods.FindByParameters(suspectParam);

	CxList loadURLHeaders = All.GetParameters(suspectMethods, 1);
	
	/* Remove loading URLs with both 'Pragma' and 'Cache-Control' set to 'no-cache':
	//HashMap<String,String> noCacheHeaders = new HashMap<String, String>(2);
	//noCacheHeaders.put("Pragma", "no-cache");
	//noCacheHeaders.put("Cache-Control", "no-cache");
	*/
	CxList safeHeaders = All.NewCxList();
	//List<string> noCache = new List<string> {"no-cache", "no_cache"};

	foreach (CxList headersMap in loadURLHeaders)
	{
		CxList reference = urAndMembers.FindAllReferences(headersMap);
		CxList putMethods = reference.GetMembersOfTarget().FindByShortName("put");
		//CxList secondSafeParam = strings.GetParameters(putMethods, 1).FindByShortNames(noCache);
		//CxList putSafeMethods = putMethods.FindByParameters(secondSafeParam);
		CxList firstSafeParam = strings.GetParameters(putMethods, 0);
		if (firstSafeParam.FindByShortName("Pragma").Count > 0 && firstSafeParam.FindByShortName("Cache-Control").Count > 0)
		{
			safeHeaders.Add(headersMap);
		}
	}
	
	suspectMethods -= suspectMethods.FindByParameters(safeHeaders);

	// Look for cache clearing when exiting the application, or when going to the background

	CxList activity = classes.FindByType("Activity");
	activity.Add(classes.InheritsFrom("Activity"));
	CxList activityMethods = Find_MethodDeclaration().GetByAncs(activity);
	CxList closeMethods = activityMethods.FindByShortNames(new List<string> {"onDestroy", "onPause", "onStop"});

	// look for invoking of WebView.clearCache(true) which clears the RAM and the chache files. 
	CxList clearCache = webViewMethods.FindByShortName("clearCache");
	CxList boolean = Find_BooleanLiteral();
	CxList valueTrue = boolean.FindByShortName("true");
	CxList trueParam = valueTrue.GetParameters(clearCache);
	CxList clearCacheTrue = clearCache.FindByParameters(trueParam);
	
	CxList clearWhenExiting = clearCacheTrue.GetByAncs(closeMethods);
	CxList clearedViews = clearWhenExiting.GetTargetOfMembers();
	CxList clearedViewsRef = urAndMembers.FindAllReferences(clearedViews);
	suspectMethods -= suspectMethods.GetMembersWithTargets(clearedViewsRef);
	result = suspectMethods;
}