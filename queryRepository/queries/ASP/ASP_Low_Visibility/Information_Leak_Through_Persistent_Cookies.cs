CxList psw = Find_Passwords();

CxList cookie = All.FindByName("*response.cookies*", false);


result = cookie.InfluencedBy(psw);