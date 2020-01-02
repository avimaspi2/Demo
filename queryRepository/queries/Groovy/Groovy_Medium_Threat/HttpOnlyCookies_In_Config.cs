CxList webXml = All.FindByFileName(@"*WEB-INF\web.xml");
CxList value_false = webXml.FindByType(typeof(StringLiteral)).FindByName("\"false\"");

CxList httpOnlyCookies_exist = webXml.FindByName("*SESSION_CONFIG.COOKIE_CONFIG.HTTP_ONLY");
CxList httpCookies_exist = webXml.FindByName("*SESSION_CONFIG.COOKIE_CONFIG");
CxList httpCookies_childs = webXml.FindByName("*SESSION_CONFIG.COOKIE_CONFIG.*");
CxList httpOnlyCookies_false = value_false.DataInfluencingOn(httpOnlyCookies_exist);

if (httpOnlyCookies_exist.Count == 0)
{
	if (httpCookies_exist.Count > 0)
	{
		result = httpCookies_exist - httpCookies_exist.GetByAncs(httpCookies_childs);
	}
	else
	{
		result = webXml.GetAncOfType(typeof(ClassDecl));
	}
}
else
{
	result = value_false * httpOnlyCookies_false;
}