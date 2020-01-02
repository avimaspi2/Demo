if (Find_HttpOnlyCookies().Count != 0)
{
	CxList declarators = Find_Declarators();
	CxList fieldDecls = Find_FieldDecls();
	CxList booleanLiterals = Find_BooleanLiteral();
	CxList possibleFlags = Find_UnknownReference();
	possibleFlags.Add(Find_MemberAccesses());
 
	CxList httpCookies = declarators.FindByType("HttpCookie");    
	CxList trueLiterals = booleanLiterals.FindByShortName("true", false);
	CxList httpOnlyFlag = trueLiterals.GetAssignee().FindByShortName("HttpOnly");
 
	CxList anonyObj = httpOnlyFlag.GetAncOfType(typeof(ObjectCreateExpr));
	CxList safeAnonyBodyCookies = anonyObj.GetAssignee();
	safeAnonyBodyCookies.Add(declarators.GetByAncs(anonyObj));
	safeAnonyBodyCookies = safeAnonyBodyCookies * httpCookies;    
	httpOnlyFlag.Add(safeAnonyBodyCookies);
 
	CxList safeHttpCookies = httpCookies.DataInfluencedBy(httpOnlyFlag)
		.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	safeHttpCookies.Add(httpCookies.FindDefinition(httpOnlyFlag.GetLeftmostTarget()));    
	safeHttpCookies.Add(safeAnonyBodyCookies * httpCookies);
 
	result = httpCookies - safeHttpCookies;
}
else {
	CxList ASPNetCoreCookiePolicyOptions = Find_CookiePolicyOptions_ASPNetCore();
	if(ASPNetCoreCookiePolicyOptions.Count > 0){
		CxList SafeHttpOnlyPolicy = Find_MemberAccesses().FindByMemberAccess("HttpOnlyPolicy.Always")
			.GetByAncs(ASPNetCoreCookiePolicyOptions);
		/*
		The purpose of this section is to identify misconfigured Cookie policy options such as:
		
		services.Configure<CookiePolicyOptions>(options => 
		        { options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.None; });
		
		It will also highlight Startup classes or configure methods where there's no configuration at all
		*/
		CxList assignedOptionField = SafeHttpOnlyPolicy.GetAssignee().FindByShortName("HttpOnly");
		result = ASPNetCoreCookiePolicyOptions - assignedOptionField.GetAncOfType(typeof(MethodInvokeExpr));
	}
}