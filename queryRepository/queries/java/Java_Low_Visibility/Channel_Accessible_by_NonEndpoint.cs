// Find all outputs that have "print*" or "write*"
CxList outputsPrintWrites = Find_Outputs();
CxList outputs = outputsPrintWrites.FindByShortName("print*");
outputs.Add(outputsPrintWrites.FindByShortName("write*"));
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
//Socket.Add(All.FindByMemberAccess("SocketChannel.open"));

//Secure 
CxList wrapSSL = All.FindByMemberAccess("SSLEngine.wrap");
//Parameters that are secure
CxList wrap_param = All.FindAllReferences(All.GetParameters(wrapSSL,1));//Get output from wrap(passed by reference)
//Outputs that use secure parameters
CxList sanitized_outputs = wrap_param.DataInfluencingOn(outputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
//Sockets that influce outputs that dont have secure parameters
CxList outputsSocket = Socket.DataInfluencingOn(outputs - sanitized_outputs);

result = outputsSocket.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);// +outputsResponse;