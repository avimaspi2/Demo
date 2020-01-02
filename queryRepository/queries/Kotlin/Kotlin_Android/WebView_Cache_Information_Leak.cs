// This query checks for usage of webViews without app cache functionality disabled.
// In this context, we consider that one webView is sanitized if one of those conditions is met:
//	* It uses setAppCacheEnabled in conjuction with setCacheMode with safe values;
//	* It disables cache usage in webView client onPageFinished method

CxList webViewDecls = Find_Declarators().FindByType("WebView");

if (webViewDecls.Count > 0) {
	CxList unknownRefs = Find_UnknownReference();
	CxList refsAndBools = unknownRefs.Clone();
	refsAndBools.Add(Find_BooleanLiteral());
	CxList members = Find_MemberAccesses();
	CxList methods = Find_Methods();
	methods.Add(Find_ObjectCreations());
	CxList methodDecls = Find_MethodDecls();
	CxList webViews = Find_Android_WebViews();
	
	CxList trueArgs = refsAndBools.FindByAbstractValue(abs => abs is TrueAbstractValue);
	CxList falseArgs = refsAndBools.FindByAbstractValue(abs => abs is FalseAbstractValue);
	CxList webSettings = Find_Android_WebViewSettings().GetMembersOfTarget();

	// First we will check if there is a sanitization both disabling cache and setting cacheMode to LOAD_NO_CACHE
	
	// Checks for sanitization in setAppCacheEnabled
	CxList sanitizedInCacheDisabled = All.NewCxList();
	CxList setAppCacheEnabled = webSettings.FindByShortNames(new List<string>{"setAppCacheEnabled", "appCacheEnabled"});
	sanitizedInCacheDisabled.Add(setAppCacheEnabled.FindByParameters(falseArgs).GetLeftmostTarget());
	sanitizedInCacheDisabled.Add((falseArgs.GetAssignee() * setAppCacheEnabled).GetLeftmostTarget());
	CxList webViewTargetRefs = sanitizedInCacheDisabled.DataInfluencedBy(webViews)
					.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	sanitizedInCacheDisabled.Add(webViewTargetRefs);

	// Checks for sanitization in setCacheMode
	CxList sanitizedInCacheMode = All.NewCxList();
	CxList loadNoCache = members.FindByMemberAccess("WebSettings.LOAD_NO_CACHE");
	CxList setCacheMode = webSettings.FindByShortNames(new List<string>{"setCacheMode", "cacheMode"});
	sanitizedInCacheMode.Add(setCacheMode.FindByParameters(loadNoCache).GetLeftmostTarget());
	sanitizedInCacheMode.Add((loadNoCache.GetAssignee() * setCacheMode).GetLeftmostTarget());
	sanitizedInCacheMode.Add(sanitizedInCacheMode.DataInfluencedBy(webViews)
					.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly));

	CxList sanitizedDefinitions = webViewDecls.FindDefinition(sanitizedInCacheDisabled);
	sanitizedDefinitions = sanitizedDefinitions * webViewDecls.FindDefinition(sanitizedInCacheMode);
	webViewDecls = webViewDecls - sanitizedDefinitions;
	
	if (webViewDecls.Count > 0) {
		// Checks for sanitization using clearCache method
		CxList clearCache = methods.FindByMemberAccess("WebView.clearCache");
		CxList clearCacheAsFalse = clearCache.FindByParameters(trueArgs).GetLeftmostTarget();
		CxList onPageFinished = methodDecls.FindByShortName("onPageFinished");
		CxList webClientClasses = clearCacheAsFalse.GetByAncs(onPageFinished).GetAncOfType(typeof(ClassDecl));
		CxList webClientUsages = methods.FindAllReferences(webClientClasses);
		webViewDecls = webViewDecls - webViewDecls.FindDefinition(members.DataInfluencedBy(webClientUsages).GetTargetOfMembers());
	}
	
	result = webViewDecls;
}