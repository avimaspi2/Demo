CxList psw = Find_Passwords();

CxList cookie = All.FindByName("*Response.Cookies*", false);
cookie.Add(All.FindByMemberAccess("HttpResponse.Cookies*", false));

result = cookie.InfluencedBy(psw);