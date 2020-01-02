//CxList encode = All.FindByMemberAccess("Server.UrlEncode") +
//	All.FindByMemberAccess("HttpUtility.UrlEncode") +
//	All.FindByMemberAccess("HttpServerUtility.UrlEncode") + 
//	All.FindByMemberAccess("AntiXss.UrlEncode");	// AntiXss

CxList encode = Find_Methods().FindByShortName("*URLencode*", false);
result = encode;