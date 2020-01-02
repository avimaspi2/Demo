// Find all outputs that have "print*" or "write*"
CxList outputs = Find_Interactive_Outputs();
outputs = 
	outputs.FindByShortName("print*") + 
	outputs.FindByShortName("write*");

/*
Bypassing:
// When the output is response, it will be checked response.write will be checked if it is not secured
CxList response = All.FindByType("HttpServletResponse").FindByType(typeof(ParamDecl));
CxList isSecure = Find_Conditions().FindByMemberAccess("HttpServletRequest.isSecure");
CxList secureIf = isSecure.GetFathers();
CxList outputsResponse = outputs - outputs.GetByAncs(secureIf);
// Find insecured response, influenced by inputs
outputsResponse = response.DataInfluencingOn(outputsResponse).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
*/

// When the output is Socket, it is not secured (not SSLSocket)
CxList Socket = All.FindByMemberAccess("Socket.getOut*").GetTargetOfMembers();
// Find Socket influenced by inputs
CxList outputsSocket = Socket.DataInfluencingOn(outputs);

result = outputsSocket.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);// +outputsResponse;