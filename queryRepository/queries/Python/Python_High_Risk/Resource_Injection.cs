CxList methods = Find_Methods();
CxList socket = Find_Members("socket.bind", methods);
CxList inputs = Find_Inputs();
CxList paths = inputs.DataInfluencingOn(socket);

result = paths.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);