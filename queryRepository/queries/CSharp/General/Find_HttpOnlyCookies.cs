CxList webConfig = Find_Web_Config().FindByFileName("*web.config");
CxList value_false = webConfig.FindByName("false").FindByType(typeof(StringLiteral));

CxList httpOnlCookies_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.HTTPCOOKIES.HTTPONLYCOOKIES");
CxList httpCookies_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.HTTPCOOKIES");
CxList httpCookies_childs = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.HTTPCOOKIES.*");
CxList httpOnlCookieysFalse = value_false.DataInfluencingOn(httpOnlCookies_exist);
CxList configuration = webConfig.FindByName("CONFIGURATION");

if (httpOnlCookies_exist.Count == 0)
{
	if (httpCookies_exist.Count > 0)
	{
		result = httpCookies_exist - httpCookies_exist.GetByAncs(httpCookies_childs);
	}
	else
	{
		result = webConfig.GetAncOfType(typeof(ClassDecl));
	}
}
else
{
	result = value_false * httpOnlCookieysFalse;
}