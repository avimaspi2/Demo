CxList stringLiterals = Find_Strings();

/* For Zend Framework */
CxList eventListeners = All.FindByType(typeof(MethodDecl)).FindByShortNames(
	new List<string>(){"routeStartup","routeShutdown","dispatchLoopStartup","preDispatch","preDispatch","dispatchLoopShutdown"});
eventListeners = All.GetByAncs(eventListeners);

/* the method setHeader is used in Zend framework to set a header in a response */
CxList setHeader = eventListeners.FindByType(typeof(MethodInvokeExpr)).FindByShortName("setHeader");

CxList firstParameterOfSetHeader = stringLiterals.GetParameters(setHeader,0);
firstParameterOfSetHeader = firstParameterOfSetHeader.FindByShortName("Strict-Transport-Security");
if(firstParameterOfSetHeader.Count > 0){
	CxList secondParameterOfSetHeader = stringLiterals.GetParameters(firstParameterOfSetHeader.GetAncOfType(typeof(MethodInvokeExpr)));
	
	CSharpGraph secondParam = secondParameterOfSetHeader.data.GetByIndex(secondParameterOfSetHeader.Count-1) as CSharpGraph;
	result.Add(secondParam.NodeId, secondParam);
}
else{
	/* For Wordpress */
	CxList headerChanges = Find_Methods().FindByShortName("header");
	
	CxList paramsOfHeader = stringLiterals.GetParameters(headerChanges);
	CxList hstsHeaders = paramsOfHeader.FindByShortName("*Strict-Transport-Security*", false);

	/* Wordpress website is sanitized if the header is set in wp-config.php */
	CxList wp_config = hstsHeaders.FindByFileName("*wp-config.php");
	if(wp_config.Count > 0){
		CSharpGraph wpConfig = wp_config.data.GetByIndex(wp_config.Count-1) as CSharpGraph;
		result.Add(wpConfig.NodeId, wpConfig);
	}
}