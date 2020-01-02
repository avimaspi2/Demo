CxList tmpFiles = All.FindByName("File.createTempFile");
tmpFiles = tmpFiles * tmpFiles.DataInfluencingOn(Find_IO());
CxList delete = All.FindByMemberAccess("File.delete*");

foreach(CxList curTmpFile in tmpFiles)
{
	if(curTmpFile.DataInfluencingOn(delete).Count == 0)
	{ // if no delete found
		// find the parameter that holds the filename, and where it has delete
		CxList fileParamName = All.GetByAncs(curTmpFile.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left);
		fileParamName = All.FindByName(fileParamName);
		fileParamName = delete.GetTargetOfMembers() * fileParamName;
		
		// find the relevant finally block.
		CxList Try = curTmpFile.GetAncOfType(typeof(TryCatchFinallyStmt));
		if (Try != null && Try.data.Count > 0)
		{
			TryCatchFinallyStmt TryGraph = Try.data.GetByIndex(0) as TryCatchFinallyStmt;
			CxList curFinally = All.FindById(TryGraph.Finally.NodeId);
			// See if there is no delete in the relevant block
			if (fileParamName.GetByAncs(curFinally).Count == 0)
			{
				result.data.AddRange(curTmpFile.data);
			}
		}
		else // If no try block at all - add to result
		{
			result.data.AddRange(curTmpFile.data);
		}
	}
}