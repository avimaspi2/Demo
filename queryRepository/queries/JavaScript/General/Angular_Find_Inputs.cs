CxList methods = Find_Methods();

// Find read from server: HttpClient.get('/addClient.html').subscribe(serverData => { this.clientSideField = serverData });
CxList httpClientMethods = methods.FindByMemberAccess("HttpClient.*");

CxList httpClientRequestsMethods = httpClientMethods.FindByMemberAccess("HttpClient.get");
httpClientRequestsMethods.Add(httpClientMethods.FindByMemberAccess("HttpClient.post"));
httpClientRequestsMethods.Add(httpClientMethods.FindByMemberAccess("HttpClient.put"));
httpClientRequestsMethods.Add(httpClientMethods.FindByMemberAccess("HttpClient.delete"));
httpClientRequestsMethods.Add(httpClientMethods.FindByMemberAccess("HttpClient.head"));
httpClientRequestsMethods.Add(httpClientMethods.FindByMemberAccess("HttpClient.jsonp"));
httpClientRequestsMethods.Add(httpClientMethods.FindByMemberAccess("HttpClient.options"));
httpClientRequestsMethods.Add(httpClientMethods.FindByMemberAccess("HttpClient.patch"));
httpClientRequestsMethods.Add(httpClientMethods.FindByMemberAccess("HttpClient.request"));

CxList potentialSubscribeCalls = httpClientRequestsMethods.GetMembersOfTarget();
CxList subscribeCalls = potentialSubscribeCalls.FindByShortName("subscribe");

CxList httpClientCallbacks = All.GetParameters(subscribeCalls, 0);

CxList httpClientCallbacksParams = All.GetParameters(httpClientCallbacks);

result = httpClientCallbacksParams;
result.Add(Angular_Find_Interactive_Inputs());