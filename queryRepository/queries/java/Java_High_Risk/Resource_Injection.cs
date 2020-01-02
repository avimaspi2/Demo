CxList socket = Find_Socket_Resource();
CxList resource = All.GetByAncs(socket);
CxList inputs = Find_Interactive_Inputs();

CxList paths = inputs.DataInfluencingOn(resource);

result = paths.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);