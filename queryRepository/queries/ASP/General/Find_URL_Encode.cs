// $ASP

CxList encode = 
	FindByMemberAccess_ASP("Server.URLEncode") +
	Find_Methods().FindByShortName("*URLEncode*", false);

result = encode;