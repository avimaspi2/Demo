// Find UDP usages (TCP is prefered instead)
CxList vulnerbaleResults = All.NewCxList();
try
{
	string[] methodsTypes = new string[]{"AsyncUdpSocket","GCDAsyncUdpSocket"};
	CxList udpType = All.FindByTypes(methodsTypes);
	CxList sockDgrm = All.FindByName("SOCK_DGRAM").FindByType(typeof(Param));
	
	vulnerbaleResults.Add(udpType);
	vulnerbaleResults.Add(sockDgrm);
}
catch (Exception error)
{
	cxLog.WriteDebugMessage(error);
}
result = vulnerbaleResults.ReduceFlowByPragma();