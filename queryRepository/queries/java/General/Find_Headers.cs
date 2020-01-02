//CxList methods=Find_Methods(); methods should be used instead of All in next line, all of the following are method invokes
CxList requestGet = All.FindByMemberAccess("request.get*");

result = All.NewCxList();
result.Add(requestGet.FindByMemberAccess("request.getHeader*"));
result.Add(requestGet.FindByMemberAccess("request.getContentType"));
result.Add(requestGet.FindByMemberAccess("request.getCharacterEncoding"));
result.Add(requestGet.FindByMemberAccess("request.getPathInfo"));
result.Add(All.FindByCustomAttribute("RequestHeader").GetFathers());

//Akka headers
result.Add(All.FindByMemberAccess("HttpCookie.get*"));
result.Add(All.FindByMemberAccess("HttpCookie.with*"));

result.Add(All.FindByMemberAccess("Cookie.getCookies"));