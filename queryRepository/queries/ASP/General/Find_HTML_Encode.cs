// $ASP

CxList encode =	FindByMemberAccess_ASP("Server.HTMLEncode");
encode.Add(Find_Methods().FindByShortName("*HTMLEncode*", false));
result = encode;