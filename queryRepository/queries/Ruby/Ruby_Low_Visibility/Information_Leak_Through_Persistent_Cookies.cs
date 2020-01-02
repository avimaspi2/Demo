CxList cgiOut = All.FindByMemberAccess("CGI.out");
//CxList cgiOutParams = All.GetByAncs(All.GetParameters(cgiOut));

CxList netHttp = All.FindByType("Net.HTTP").GetMembersOfTarget().FindByShortName("post");
//CxList netHttpParams = All.GetByAncs(All.GetParameters(netHttp));

//CxList cookies = cgiOutParams.FindByShortName("cookie") + netHttpParams.FindByShortName("data");
//cookies = cookies.FindByType(typeof(StringLiteral));

CxList cookieParams = All.GetParameters(cgiOut) + All.GetParameters(netHttp);
CxList personal = Find_Personal_Info();

result = personal.DataInfluencingOn(cookieParams);