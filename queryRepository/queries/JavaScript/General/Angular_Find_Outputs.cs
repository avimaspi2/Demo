CxList methods = Find_Methods();

result.Add(methods.FindByMemberAccess("HttpClient.*"));
result.Add(methods.FindByMemberAccess("HttpHandler.handle"));
result.Add(Find_Framework_Outputs());