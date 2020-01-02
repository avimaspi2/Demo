CxList sockets = All.FindByType("Socket");
CxList receivers = sockets.GetMembersOfTarget().FindByShortName("Receive");
CxList dataReceived = All.GetParameters(receivers, 0);

result = dataReceived;