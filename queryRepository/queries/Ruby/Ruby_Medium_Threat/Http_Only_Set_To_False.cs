CxList hOnly=All.FindByShortName("http_only") + All.FindByShortName("httponly") +
	All.FindByShortName("session_http_only");
result=All.FindByType(typeof(BooleanLiteral)).FindByShortName("false").DataInfluencingOn(hOnly);