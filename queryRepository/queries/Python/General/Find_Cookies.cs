CxList methods = Find_Methods();
CxList strings = Find_Strings();

CxList os = Find_Members("os.environ");
CxList stringLiteral = strings.FindByName("'HTTP_COOKIE'");
stringLiteral.Add(strings.FindByName("\"HTTP_COOKIE\""));

// cookies
CxList cookiesMethodsAll = stringLiteral.GetByAncs(os).GetAncOfType(typeof(IndexerRef));
cookiesMethodsAll.Add(Find_Methods_By_Import("Cookie", new string[]{ "SimpleCookie"}));
cookiesMethodsAll.Add(Find_Methods_By_Import("cookies", new string[]{ "SimpleCookie"}));
cookiesMethodsAll.Add(Find_Methods_By_Import("Cookie", new string[]{ "BaseCookie"}));
cookiesMethodsAll.Add(Find_Methods_By_Import("cookies", new string[]{ "BaseCookie"}));
cookiesMethodsAll.Add(Find_Methods_By_Import("http.cookies", new string[]{ "SimpleCookie"}));
cookiesMethodsAll.Add(Find_Methods_By_Import("http.cookies", new string[]{ "BaseCookie"}));
cookiesMethodsAll.Add(Find_Members("request.COOKIES"));

CxList cookiesMethods = cookiesMethodsAll.GetAncOfType(typeof(AssignExpr)); 
cookiesMethods.Add(cookiesMethodsAll.GetAncOfType(typeof(Declarator))); // Assign or declare

CxList cookies = All.GetByAncs(cookiesMethods).FindByAssignmentSide(CxList.AssignmentSide.Left);
cookies.Add(methods.FindByName("*.set_cookie")); // assigned to

result = cookies;