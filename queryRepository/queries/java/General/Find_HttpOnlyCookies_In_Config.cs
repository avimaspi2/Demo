String findInConfig = "false";
if (param.Length > 0)
{
	findInConfig = param[0] as String;
}

CxList webXml = Find_Web_Xml();
CxList value_false = webXml.FindByType(typeof(StringLiteral)).FindByName(findInConfig);

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

CxList weblogicXml = Find_Weblogic_Xml();
CxList weblogicValueFalse = weblogicXml.FindByType(typeof(StringLiteral)).FindByName(findInConfig);
CxList weblogicCookieHttpOnlyExist = weblogicXml.FindByName("*SESSION_DESCRIPTOR.COOKIE_HTTP_ONLY");
CxList cookieHttpOnlyFalse = weblogicValueFalse.DataInfluencingOn(weblogicCookieHttpOnlyExist);

if (weblogicCookieHttpOnlyExist.Count > 0)
{
	result.Add(weblogicValueFalse * cookieHttpOnlyFalse);
}