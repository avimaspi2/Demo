CxList cookies = 
	All.FindByMemberAccess("request.getCookies");
cookies.Add(Find_CookieValue_Annotation());

CxList cond = Find_Conditions();

// Ignore conditions like if (cookie != null)
CxList sanitizers = Find_NullLiteral();
sanitizers.Add(All.FindByMemberAccess("*.length", false));
sanitizers.Add(All.FindByMemberAccess("*.Count", false));
sanitizers = sanitizers.GetFathers();

result = cond.InfluencedByAndNotSanitized(cookies, sanitizers);