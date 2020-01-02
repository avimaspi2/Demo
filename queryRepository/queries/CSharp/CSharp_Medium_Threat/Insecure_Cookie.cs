///////////////////////////////////////////////////////////////////////////
// Query: Insecure_Cookie
// Purpose: Find cookies that don't reqire SSL
// 			First find is web.config includes requireSSL="true"
//			If it doesn't, find cookies that don't call HttpCookie.Secure 
///////////////////////////////////////////////////////////////////////////
CxList webconfig = Find_Web_Config();

CxList trueLiteral = webconfig.FindByShortName("true");
CxList requireSSL = webconfig.FindByMemberAccess("HTTPCOOKIES.REQUIRESSL");

bool safeConfiguration = (requireSSL.InfluencedBy(trueLiteral).Count > 0);

CxList ASPNetCoreCookiePolicyOptions = Find_CookiePolicyOptions_ASPNetCore();
// if configuration doesn't enforce using SSL, make sure HttpCookie.Secure is called
if(!safeConfiguration && ASPNetCoreCookiePolicyOptions.Count == 0)
{
	CxList trueBool = All.FindByType(typeof(BooleanLiteral)).FindByShortName("true");
	CxList trueAssigned = trueBool.FindByAssignmentSide(CxList.AssignmentSide.Right);
	
	
	CxList cookies = All.FindByType("HttpCookie").FindByType(typeof(Declarator));

	CxList personalInfo = Find_Personal_Info();	
		
	CxList allref = All.FindAllReferences(cookies);
	
	CxList sensitiveCookies = allref.InfluencedBy(personalInfo - allref);

	sensitiveCookies = sensitiveCookies.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
	sensitiveCookies = sensitiveCookies.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	sensitiveCookies.Add(personalInfo * cookies);
	foreach(CxList cookie in sensitiveCookies)
	{
		CxList cookRef = allref.FindAllReferences(cookie);
		CxList secure = cookRef.GetMembersOfTarget().FindByShortName("Secure");
		
		if(trueAssigned.GetByAncs(secure.GetFathers()).Count == 0)
		{
			result.Add(cookie);
		}		
	}
}else if(ASPNetCoreCookiePolicyOptions.Count > 0){
	CxList memberAccesses = Find_MemberAccesses();
	CxList SafeCookieSecurityPolicy = memberAccesses.FindByMemberAccess("CookieSecurePolicy.Always");
	SafeCookieSecurityPolicy.Add(memberAccesses.FindByMemberAccess("CookieSecurePolicy.SameAsRequest"));
	/*
	The purpose of this section is to identify misconfigured Cookie policy options such as:
	
	services.Configure<CookiePolicyOptions>(options =>
           {
               options.Secure = CookieSecurePolicy.None;
           });
	
	It will also highlight Startup classes or configure methods where there's no configuration at all
	*/
	CxList AssignedOptionField = SafeCookieSecurityPolicy.GetAssignee().FindByShortName("Secure");
	result = ASPNetCoreCookiePolicyOptions - AssignedOptionField.GetAncOfType(typeof(MethodInvokeExpr));
}