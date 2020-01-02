//CxList encode = All.FindByMemberAccess("server.urlencode") +
//	All.FindByMemberAccess("httputility.urlencode") +
//	All.FindByMemberAccess("httpserverutility.urlencode") +
//	All.FindByMemberAccess("antixss.urlencode");	// AntiXss

CxList encode = Find_Methods().FindByShortName("*urlencode*", false);
result = encode;