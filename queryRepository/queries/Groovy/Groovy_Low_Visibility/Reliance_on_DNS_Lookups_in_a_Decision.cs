CxList cond = Find_Conditions();

CxList ip = All.FindByMemberAccess("request.getRemoteAddr");
CxList inetAddress = 
	All.FindByMemberAccess("InetAddress.getByName") +
	All.FindByMemberAccess("InetAddress.getByAddress");

result = cond.DataInfluencedBy(inetAddress).DataInfluencedBy(ip);