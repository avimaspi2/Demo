// Find unchecked file operations
// Make sure result code of all file operations are checked.
CxList vulnerbaleResults = All.NewCxList();
try
{
	CxList methods = Find_Methods();
	CxList writesToFile = methods.FindByShortName("writeToFile:*");
	writesToFile.Add(methods.FindByShortName("write:*").FindByParameterName("toFile", 0));
	CxList writesToURL = methods.FindByShortName("writeToURL:*");	
	writesToURL.Add(methods.FindByShortName("write:*").FindByParameterName("toUrl", 0));


	CxList conditions = Get_Conditions();
	
	// filter methods calls which are part of the conditions expressions
	writesToFile = writesToFile - (writesToFile * conditions);
	writesToURL = writesToURL - (writesToURL * conditions);
	
	// filter methods calls which impacts on conditions expressions
	CxList uncheckedWritesToFile = writesToFile - conditions.DataInfluencedBy(All.GetParameters(writesToFile, 0))
		.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow)
		.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllButNotStartAndEnd).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);	
	CxList uncheckedWritesToURL = writesToURL - conditions.DataInfluencedBy(All.GetParameters(writesToURL, 0))
		.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow)
		.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllButNotStartAndEnd).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	
	vulnerbaleResults.Add(uncheckedWritesToFile);
	vulnerbaleResults.Add(uncheckedWritesToURL);
}
catch (Exception error)
{
	cxLog.WriteDebugMessage(error);
}
result = vulnerbaleResults;