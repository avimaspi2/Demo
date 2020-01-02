CxList httpOnlyCookies = null;
if (param.Length > 0)
{
	httpOnlyCookies = param[0] as CxList;
}
if (httpOnlyCookies == null)
{
	httpOnlyCookies = Find_BooleanLiteral().FindByShortName("true");
}

CxList cookieSetHeader = Find_Methods().FindByMemberAccess("Cookie.setHttpOnly");
return cookieSetHeader.FindByParameters(httpOnlyCookies);