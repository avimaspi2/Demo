//CxList encode = All.FindByMemberAccess("Server.HtmlEncode") + 
//	All.FindByMemberAccess("HttpUtility.HtmlEncode") + 
//	All.FindByMemberAccess("HttpServerUtility.HtmlEncode") + 
//	All.FindByMemberAccess("AntiXss.HtmlEncode");	// AntiXss

CxList encode = Find_Methods().FindByShortName("*HTMLencode*", false);
result = encode;