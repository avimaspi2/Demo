CxList inputs = Find_Interactive_Inputs();

if (param.Length > 0)
{
	try
	{
		if (param[0] != null)
			inputs = param[0] as CxList;
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex.Message);
	}
}
	
/*
	In order to differentiate results between Absolute_Path_Traversal and Relative_Path_Traversal,
	only inputs which are prepended some path before hitting the file handling statement are considered 
	a Relative_Path_Traversal vulnerability. Therefore Find_Path_Prepending_Operations's results are
	required intermediate nodes on the path between the input and the file handling statements.
*/
CxList pathPrependingOperations = Find_Path_Prepending_Operations();

/*
	Returns methods replacing both OS's file separators ("\" and "/").
*/
CxList pathTraversalSanitizers = Find_Path_Traversal_Sanitize();

CxList fileOpeningStatements = Find_Files_Open();
fileOpeningStatements.Add(Find_Java7_Files_Open());

CxList pathTraversal = fileOpeningStatements.InfluencedByAndNotSanitized(inputs, pathTraversalSanitizers)
	.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
/*
	Select path traversal flows containing elements of pathPrependingOperations.
*/
CxList relativePathTraversal = All.NewCxList();
foreach (CxList pt in pathTraversal.GetCxListByPath())
{
	CxList flowNodes = pt.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllNodes);
	if((flowNodes * pathPrependingOperations).Count > 0){
		relativePathTraversal.Add(pt);
	}
}

result = relativePathTraversal;