CxList psw = Find_Passwords();

CxList cookie = All.FindByName("*Response.Cookies*", false) + 
				All.FindByMemberAccess("HttpResponse.Cookies*");


result = cookie.InfluencedBy(psw);