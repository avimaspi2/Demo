CxList responseObject = Hapi_Find_Response_Objects();
responseObject.Add(Hapi_Find_Reply_Interface());

CxList redirects = responseObject.GetMembersOfTarget().FindByShortName("redirect");

result = All.GetParameters(redirects, 0);