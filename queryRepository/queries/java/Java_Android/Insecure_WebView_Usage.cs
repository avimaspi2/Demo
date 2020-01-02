/////////////////////////////////////////////////////////////////////////////////////////////
// CERT DRD02-J. Do not allow WebView to access sensitive local resource through file scheme
// this query looks for a WebView invocation of an address originated from an input,
// when the settings allows JS or file access.
// URI received from outside a trust boundary should be validated before rendering.
// Sanitization in this case is checking if the address start with "file://".
// this query also looks for a WebView declaration that enables Plugin support.
/////////////////////////////////////////////////////////////////////////////////////////////

CxList methods = Find_Methods();
CxList allParams = Find_Params();
CxList members = Find_MemberAccess();
CxList unknownRef = Find_UnknownReference();
CxList boolean = Find_BooleanLiteral();

CxList urAndMembers = All.NewCxList(); 
urAndMembers.Add(unknownRef);
urAndMembers.Add(members);

CxList webViews = All.FindByType("WebView");
webViews.Add(urAndMembers.FindByType(Find_Class_Decl().InheritsFrom("WebView")));

CxList webSettings = webViews.GetMembersOfTarget().FindByShortName("getSettings");

CxList membersUnknownRef = All.NewCxList();
membersUnknownRef.Add(members);
membersUnknownRef.Add(unknownRef);

webSettings.Add(membersUnknownRef.FindAllReferences(webSettings.GetAssignee()));
CxList webSettingsMethods = webSettings.GetMembersOfTarget();

////WebSettings.setJavaScriptEnabled
CxList jsEnabled = methods.FindByMemberAccess("WebSettings.setJavaScriptEnabled");
jsEnabled.Add(webSettingsMethods.FindByShortName("setJavaScriptEnabled"));
CxList trueLiteral = boolean.FindByShortName("true").GetByAncs(jsEnabled);

// Find WebSettings.setJavaScriptEnabled(true);
CxList jsEnabledTrue = trueLiteral.GetAncOfType(typeof(MethodInvokeExpr)) * jsEnabled;

////WebSettings.setAllowFileAccess
CxList allowFileAccess = methods.FindByMemberAccess("WebSettings.setAllowFileAccess");
allowFileAccess.Add(webSettingsMethods.FindByShortName("setAllowFileAccess"));
CxList falseLiteral = boolean.FindByShortName("false").GetByAncs(allowFileAccess);

//Find WebSettings.setAllowFileAccess(false)
CxList allowFileAccessFalse = falseLiteral.GetAncOfType(typeof(MethodInvokeExpr)) * allowFileAccess;

//Search all WebViews that don't have WebSettings.setAllowFileAccess(false)
CxList protectedWebViews = webViews.DataInfluencingOn(allowFileAccessFalse).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
CxList allowFileAccessEnabled = webViews - protectedWebViews;

// If JavaScript or file access are enabled, find path from input to loadUrl which is not sanitized
if(jsEnabledTrue.Count > 0 || allowFileAccessEnabled.Count > 0)
{
	// If checking that URL starts with "file:" then it is sanitized
	CxList fileUrls = Find_Strings().FindByName("file://*"); // "file://" strings 
	// string variables with "file://" strings :
	CxList vars = urAndMembers.FindByAbstractValues(fileUrls); 

	CxList fileUrlsVars = All.NewCxList();
	fileUrlsVars.Add(fileUrls);
	fileUrlsVars.Add(vars);
	
	 // string vars that check start with("file://") :
	CxList fileSanitizers = methods.FindByMemberAccess("String.startsWith").FindByParameters(fileUrlsVars).GetTargetOfMembers();
	CxList ifStmt = Find_Ifs(); // if statements
	CxList directChildren = All.FindByFathers(ifStmt); // all children of all ifs, including their if-true-block
	CxList ifBlocks = directChildren.FindByType(typeof(StatementCollection));
	// all condition statements of ifs :
	CxList conditionOnly = directChildren - ifBlocks; 
	// if that have start with("file://") conditions :
	CxList marker = fileSanitizers.GetByAncs(conditionOnly).GetAncOfType(typeof(IfStmt)); 
	// the if-blocks that check a start with("file://") condition :
	CxList markerStmtCol = ifBlocks.FindByFathers(marker); 
	CxList sanitizer = All.GetByAncs(markerStmtCol); // the closure of these if-blocks is sanitized
	CxList loadUrl = webViews.GetMembersOfTarget().FindByShortName("loadUrl");
	CxList inputs = Find_Inputs(); 
	result = loadUrl.InfluencedByAndNotSanitized(inputs, sanitizer);
}

////WebSettings.setPluginState
CxList pluginState = methods.FindByMemberAccess("WebSettings.setPluginState");
pluginState.Add(webSettingsMethods.FindByShortName("setPluginState"));

// Find WebSettings.setPluginState(PluginState.ON) or
// WebSettings.setPluginState(PluginState.ON_DEMAND)
CxList pluginParameters = allParams.GetParameters(pluginState).FindByShortName("ON*");
result.Add(pluginState.FindByParameters(pluginParameters));