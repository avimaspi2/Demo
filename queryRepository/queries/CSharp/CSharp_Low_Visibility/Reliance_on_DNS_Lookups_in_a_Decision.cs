CxList cond = Find_Conditions();

CxList hostIPAddress = All.FindByMemberAccess("IPAddress.Parse");
CxList IPHostEntry = All.FindByMemberAccess("Dns.GetHostByAddress");
CxList IPHostEntryName = All.FindByMemberAccess("Dns.GetHostName");

CxList DNS = IPHostEntry.InfluencedBy(hostIPAddress);
DNS.Add(IPHostEntryName);

result = cond.InfluencedBy(DNS);