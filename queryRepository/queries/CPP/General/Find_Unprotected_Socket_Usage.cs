//	Find_Unprotected_Socket_Usage
//  -----------------------
//  In this query we look for a transmission of information
//  that is not protected by SSL. 
///////////////////////////////////////////////////////////////////////

CxList methods = Find_Methods();

CxList socket = methods.FindByShortName("socket");
socket.Add(All.FindByType("QTcpSocket"));

List <string> outputNames = new List<string> {
		"write",				
		"sendto",				
		"send",	
		"sendmsg"	
		};
CxList send = methods.FindByShortNames(outputNames);

CxList sendToSocket = send.DataInfluencedBy(socket).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

result = sendToSocket;