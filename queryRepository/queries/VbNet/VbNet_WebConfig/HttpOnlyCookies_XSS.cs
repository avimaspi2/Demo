CxList webConfig = Find_Web_Config();
CxList value_false = webConfig.FindByName("false", false).FindByType(typeof(StringLiteral));

CxList httpOnlCookies_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.HTTPCOOKIES.HTTPONLYCOOKIES", false);
CxList httpCookies_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.HTTPCOOKIES", false);
CxList httpCookies_childs = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.HTTPCOOKIES.*", false);
CxList httpOnlCookieysFalse = value_false.DataInfluencingOn(httpOnlCookies_exist);
CxList configuration = webConfig.FindByName("CONFIGURATION", false);

if (httpOnlCookies_exist.Count == 0)
{
	if (httpCookies_exist.Count > 0)
	{
		result = httpCookies_exist - httpCookies_exist.GetByAncs(httpCookies_childs);
	}
}
else
{
	result = value_false * httpOnlCookieysFalse;
}