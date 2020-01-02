CxList socket = Find_Object_Create().FindByShortName("ServerSocket");
CxList resource = All.GetByAncs(socket);
CxList input = Find_Potential_Inputs();
result = input.DataInfluencingOn(resource);