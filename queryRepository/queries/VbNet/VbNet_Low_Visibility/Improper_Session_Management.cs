CxList sid = All.FindByName("*Request.QueryString_sid*", false);
sid.Add(All.FindByName("*Request.QueryString_session*", false));

CxList queryString = All.FindByMemberAccess("HttpRequest.QueryString_*", false);
queryString.Add(All.FindByName("*Request.QueryString_*", false));

result = sid.GetParameters(queryString);