CxList methods = Find_Methods();
CxList strings = Find_Strings();

CxList trueStmts = All.FindByShortName("true", false);

CxList ini_set = methods.FindByShortName(@"ini_set");
ini_set = ini_set.FindByParameters(strings.FindByShortName("session.cookie_httponly"));
ini_set = ini_set.FindByParameters(All.FindByType(typeof(IntegerLiteral)).FindByShortName("1"));
CxList session_set = methods.FindByShortName(@"session_set_cookie_params");
CxList session_set_true = trueStmts.GetParameters(session_set, 4);
session_set = session_set.FindByParameters(session_set_true);

if ((ini_set + session_set).Count == 0)
{
	CxList cookiesMethods = methods.FindByShortNames(new List<String>(){ @"setcookie", @"setrawcookie" });
	//If the last parameter is omitted it's set to false
	foreach (CxList cookieMethod in cookiesMethods) 
	{  
		if(All.GetParameters(cookieMethod).FindByType(typeof(Param)).Count < 7)
		{
			result.Add(cookieMethod);
		}
	}
	//evaluate if the last parameter is False
	CxList lastParameters = All.GetParameters(cookiesMethods, 6);
    CxList falseAbstractValues = lastParameters.FindByAbstractValue(abstractValue => abstractValue is FalseAbstractValue);
	result.Add(falseAbstractValues);
}