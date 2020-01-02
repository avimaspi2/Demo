CxList methods = Find_Methods();
CxList members = Find_MemberAccesses();
CxList unknownRef = Find_UnknownReference();
CxList urAndMembers = All.NewCxList();
urAndMembers.Add(unknownRef);
urAndMembers.Add(members);

CxList boolean = Find_BooleanLiteral();

// Find all WebView instances
CxList webViews = All.FindByType("WebView");
webViews.Add(urAndMembers.FindByType(Find_ClassDecl().InheritsFrom("WebView")));

// WebSettings instances (ex: webView.getSettings())
CxList webSettings = webViews.GetMembersOfTarget().FindByShortNames(new List<String>() {"getSettings", "settings"});
webSettings.Add(urAndMembers.FindAllReferences(webSettings.GetAssignee()));
CxList webSettingsMethods = webSettings.GetMembersOfTarget();

// Method/property used to enable javascript
CxList jsEnabled = All.NewCxList();
jsEnabled.Add(webSettingsMethods.FindByShortNames(new List<string>() {"setJavaScriptEnabled", "javaScriptEnabled"}));

// Get assigns/sets using true of javascriptEnabled
CxList trueLiteral = boolean.FindByShortName("true");
CxList trueLiteralInMethodArg = trueLiteral.GetByAncs(jsEnabled).GetAncOfType(typeof(MethodInvokeExpr));

CxList allTrueLiteral = All.NewCxList();
allTrueLiteral.Add(trueLiteralInMethodArg);
allTrueLiteral.Add(trueLiteral.GetAssignee());

CxList jsEnabledTrue = allTrueLiteral * jsEnabled;

// Check for file access in WebView
CxList allowFileAccess = methods.FindByMemberAccess("WebSettings.setAllowFileAccess");
allowFileAccess.Add(webSettingsMethods.FindByShortNames(new List<string>() {"setAllowFileAccess", "allowFileAccess"}));
CxList falseLiteral = boolean.FindByShortName("false");
CxList falseLiteralInMethodArg = falseLiteral.GetByAncs(allowFileAccess).GetAncOfType(typeof(MethodInvokeExpr));

CxList allFalseLiteral = All.NewCxList();
allFalseLiteral.Add(falseLiteralInMethodArg);
allFalseLiteral.Add(falseLiteral.GetAssignee());

CxList allowFileAccessFalse = allFalseLiteral * allowFileAccess;

// Search all WebViews that don't have WebSettings.setAllowFileAccess(false)
CxList protectedWebViews = webViews.DataInfluencingOn(All.FindAllReferences(allowFileAccessFalse.GetLeftmostTarget()));
protectedWebViews = unknownRef.FindAllReferences(protectedWebViews);
CxList allowFileAccessEnabled = webViews.FindByType(typeof(UnknownReference)) - protectedWebViews;

// If JavaScript or file access are enabled, find path from input to loadUrl which is not sanitized
if (jsEnabledTrue.Count > 0 || allowFileAccessEnabled.Count > 0)
{
	CxList loadUrl = webViews.GetMembersOfTarget().FindByShortName("loadUrl");	
	// If the loadUrl method is used inside a if condition that checks for possible sanitizations
	// (ex if (input.startsWith("file://"))), we consider it is sanitized,
	CxList santitizerStrs = Find_Strings().FindByShortNames(new List<string>(){ "https://*", "file://*" });
	santitizerStrs.Add(urAndMembers.FindByAbstractValues(santitizerStrs)); 

	CxList startsWithSanitizer = methods.FindByShortName("startsWith").FindByParameters(santitizerStrs).GetTargetOfMembers();
	CxList ifStmt = Find_Ifs(); // if statements
	CxList directChildren = All.FindByFathers(ifStmt); // All children of all ifs, including their if-true-block
	CxList ifBlocks = directChildren.FindByType(typeof(StatementCollection));
	CxList conditionOnly = directChildren - ifBlocks; 
	CxList marker = startsWithSanitizer.GetByAncs(conditionOnly).GetAncOfType(typeof(IfStmt)); 
	CxList markerStmtCol = ifBlocks.FindByFathers(marker);

	CxList sanitizer = loadUrl.GetByAncs(markerStmtCol);
	CxList inputs = Find_Inputs();
	result = loadUrl.InfluencedByAndNotSanitized(inputs, sanitizer);
	
	loadUrl -= loadUrl.GetByAncs(markerStmtCol);
	
	// This allows us to catch instances of variables where there is no flow between the assignment of an input to the variable
	// and the usage of the variable as a parameter of "WebView.loadUrl" method.
	CxList inputMembers = inputs.GetMembersOfTarget();
	CxList allReferences = unknownRef.FindAllReferences(All.FindDefinition(inputMembers.GetAssignee()));
	result.Add(loadUrl.FindByParameters(allReferences));	
}

// WebSettings.setPluginState() / WebSettings.pluginState = ?
CxList pluginState = methods.FindByMemberAccess("WebSettings.setPluginState");
pluginState.Add(webSettingsMethods.FindByShortNames(new List<string>(){"setPluginState", "pluginState"}));

CxList onOrOnDemandAccess = members.FindByShortName("ON*");
CxList pluginParameters = onOrOnDemandAccess.GetParameters(pluginState);
result.Add(pluginState.FindByParameters(pluginParameters));
result.Add(pluginState * onOrOnDemandAccess.GetAssignee());