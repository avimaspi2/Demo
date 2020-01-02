CxList socket = Find_Object_Create().FindByShortName("ServerSocket");
CxList resource = All.GetByAncs(socket);
CxList inputs = Find_Interactive_Inputs();

CxList paths = inputs.DataInfluencingOn(resource);

result = paths.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);