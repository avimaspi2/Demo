if (param.Length > 0)
{
	CxList Inputs = param[0] as CxList;
	
	CxList obj = Find_Object_Create().FindByShortName("File*");
	// This create methods comes from the java.nio.file
	CxList methods = Find_Methods();
	obj.Add(methods.FindByMemberAccess("Files.newInputStream"));
	obj.Add(methods.FindByMemberAccess("Files.newOutputStream"));
	obj.Add(methods.FindByMemberAccess("Files.newBufferedReader"));
	obj.Add(methods.FindByMemberAccess("Files.newBufferedWriter"));
	obj.Add(methods.FindByMemberAccess("Files.newByteChannel"));
	
	CxList objPath = obj + All.FindAllReferences(obj);
	objPath = All.GetByAncs(objPath) + objPath.GetAncOfType(typeof(AssignExpr));
	
	CxList sanitize = Find_Path_Traversal_Sanitize();
	CxList fullResult = obj.InfluencedByAndNotSanitized(Inputs, sanitize);
	
	sanitize.Add(Find_NonLeft_Binary(objPath));
	CxList newFiles = fullResult.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	result = fullResult - fullResult.InfluencedByAndNotSanitized(newFiles, sanitize);
		
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}