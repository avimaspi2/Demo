CxList methodInvokes = Find_Methods();

// WIN32 Winsock:
CxList winsock = methodInvokes.FindByShortNames(new List<string> {"recv", "recvfrom"});
result.Add(All.GetParameters(winsock, 1));

// MFC CSocket:
// CSocket/CAsyncSocket.Receive/ReceiveFrom/ReceiveFromEx
CxList mfcSocket = methodInvokes.FindByShortNames(new List<string> {"Receive", "ReceiveFrom", "ReceiveFromEx"});
result.Add(All.GetParameters(mfcSocket, 0));

// .Net System.Net.Sockets:
//	Socket.Receive
// 	Socket.ReceiveAsync 
// 	Socket.ReceiveFrom 
// 	Socket.ReceiveFromAsync 
// 	Socket.ReceiveMessageFrom 
// 	Socket.ReceiveMessageFromAsync
CxList dotNetSockets = methodInvokes.FindByMemberAccess("Socket.Receive*");
result.Add(All.GetParameters(dotNetSockets, 0));