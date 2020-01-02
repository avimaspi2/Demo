CxList responseObject = Hapi_Find_Response_Objects();
responseObject.Add(Hapi_Find_Server_Instance());

result = All.GetParameters(responseObject.GetMembersOfTarget().FindByShortName("state"));