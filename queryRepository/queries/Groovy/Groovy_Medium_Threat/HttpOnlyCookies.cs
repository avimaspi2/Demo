if (HttpOnlyCookies_In_Config().Count != 0)
{
	CxList httpCookies = All.FindByType("Cookie").FindByType(typeof(Declarator));
	CxList httpOnlyCookies = All.FindByType(typeof(BooleanLiteral)).FindByShortName("true");
	httpOnlyCookies = All.FindByMemberAccess("Cookie.setHttpOnly").DataInfluencedBy(httpOnlyCookies);
	result = httpCookies - httpCookies.FindDefinition(httpOnlyCookies.GetTargetOfMembers());
}