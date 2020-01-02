CxList server = Hapi_Find_Server_Instance();
server.Add(Hapi_Find_Request_Objects());
result = server.GetMembersOfTarget().FindByShortName("log");