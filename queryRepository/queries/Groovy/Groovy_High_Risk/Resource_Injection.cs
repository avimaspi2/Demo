CxList socket = Find_Object_Create().FindByShortName("ServerSocket");
CxList resource = All.GetByAncs(socket);
CxList inputs = Find_Interactive_Inputs();

result = inputs.DataInfluencingOn(resource);