// Find all cases where data received from network (including https) influences on a url
CxList vulnerableResults = All.NewCxList();
try
{	
	CxList http = Find_Pure_Http();
	CxList https = Find_HTTPS();	
	
	CxList httpAll = All.NewCxList();
	httpAll.Add(http);
	httpAll.Add(https);
	
	CxList networkSource = Find_Downloaded_Data(httpAll);	
		
	CxList urls = All.FindByTypes(new String[]{"NSURL","URL"});
	CxList impactPaths = urls.DataInfluencedBy(networkSource);
	vulnerableResults = impactPaths.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}
catch (Exception error)
{
	cxLog.WriteDebugMessage(error);
}
result = vulnerableResults;