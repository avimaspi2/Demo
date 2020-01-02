CxList createExpr = All.FindByType(typeof(ObjectCreateExpr));

CxList inputs = Find_Interactive_Inputs();
CxList socket = 
	All.GetByAncs(createExpr.FindByShortName("TcpListener")) +
	All.FindByMemberAccess("Socket.Connect");

result = inputs.DataInfluencingOn(socket);