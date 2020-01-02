CxList psw = Find_Passwords();
CxList methods = Find_Methods();
CxList cookie = methods.FindByName("*setcookie*", false);
cookie.Add(methods.FindByName("*setrawcookie*", false));

List<String> cookie_arrays = new List<String>(){ "*_COOKIE*", "*HTTP_COOKIE_VARS" };

result = cookie.FindByParameters(psw);
result.Add(psw.GetAncOfType(typeof(IndexerRef)).FindByShortNames(cookie_arrays, false));