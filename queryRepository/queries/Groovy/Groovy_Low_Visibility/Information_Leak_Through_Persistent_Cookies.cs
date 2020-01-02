CxList psw = Find_Passwords();

CxList cookie = All.FindByName("*.setCookies*", false);

result = cookie.InfluencedBy(psw);