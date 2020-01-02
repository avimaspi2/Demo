CxList cond = Find_Conditions();

CxList ip = All.FindByMemberAccess("request.getRemoteAddr");

CxList inetAddress = All.FindByMemberAccess("InetAddress.getByName");
inetAddress.Add(All.FindByMemberAccess("InetAddress.getByAddress"));

result = cond.DataInfluencedBy(inetAddress).DataInfluencedBy(ip);