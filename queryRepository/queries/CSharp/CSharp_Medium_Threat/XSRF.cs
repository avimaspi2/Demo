if (All.isWebApplication)
{
	CxList aspSanitized = Find_XSRF_Sanitize();
	CxList viewstate_oninit = aspSanitized.FindByShortName("OnInit");
	viewstate_oninit.Add(aspSanitized.FindByShortName("Page_Init"));	
	
	if (viewstate_oninit.Count == 0){
		//remoting is not a valid source for XSRF
		CxList requests = Find_Interactive_Inputs() - Find_Remoting_Inputs();
		requests.Add(All.FindByName("*Request.QueryString*"));
		//Remove sanitized ASP inputs (inputs that are within sanitized methods)
		requests -= requests.GetByAncs(aspSanitized);
	
		CxList db = Find_DB_In();
		CxList strings = Find_Strings();

		CxList write = strings.FindByName("*update*", StringComparison.OrdinalIgnoreCase);
		write.Add(strings.FindByName("*delete*", StringComparison.OrdinalIgnoreCase));
		write.Add(strings.FindByName("*insert*", StringComparison.OrdinalIgnoreCase));

		// The web services may be used as Input to DB
		CxList webService = Find_Web_Services();	
		CxList webServiceGetMembersOfTarget = webService.GetMembersOfTarget();
		CxList webServiceMethods = webServiceGetMembersOfTarget.FindByType(typeof(MethodInvokeExpr));
		
		// The typical input method name can't start with "Get", "IsValid", "Check"
		CxList irrelevanetWebServiceMethods = webServiceGetMembersOfTarget.FindByName("*Get*", false);
		irrelevanetWebServiceMethods.Add(webServiceGetMembersOfTarget.FindByName("*IsValid*", false));
		irrelevanetWebServiceMethods.Add(webServiceGetMembersOfTarget.FindByName("*Check*", false));
		
		irrelevanetWebServiceMethods = irrelevanetWebServiceMethods.FindByType(typeof(MethodInvokeExpr));
		
		CxList relevantWebServices = webServiceMethods - irrelevanetWebServiceMethods;
		CxList sanitizers = Find_Boolean_Operators();
				
		result = db.InfluencedByAndNotSanitized(write, sanitizers).InfluencedByAndNotSanitized(requests, sanitizers);
		result.Add(All.GetParameters(relevantWebServices).InfluencedByAndNotSanitized(requests, sanitizers));
		
		// Finds XSRF in ASP or MVC views and controllers
		result.Add(Find_ASP_MVC_XSRF());
	}
}