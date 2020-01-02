/* 
	This query searched for opening file statements which path is influenced by an input.
 	By default, the query will return Absolute_Path_Traversal, but the input lists
 	can be overridden to reflect stored path traversal as well.
 	param[0] is CxLIst inputs (or Find_Interactive_Inputs() by default)
	The rest of the parameters are ignored.
*/
CxList inputs = Find_Interactive_Inputs();
CxList methods = Find_Methods();
CxList decls = Find_Declarators();
CxList unknownRef = Find_UnknownReference();

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
	Filter the Properties.get that are affected by a Properties.load or loadFromXML
*/
CxList loadProperties = methods.FindByMemberAccess("Properties.load");
loadProperties.Add(methods.FindByMemberAccess("Properties.loadFromXML"));
CxList originalGets = inputs.FindByShortName("get");
CxList getProps = originalGets.GetTargetOfMembers();
CxList def = decls.FindDefinition(getProps);
CxList filteredLoads = loadProperties.FindInScope(def, getProps);
CxList filteredProps = filteredLoads.GetTargetOfMembers();
CxList filteredGets = unknownRef.FindAllReferences(filteredProps).GetMembersOfTarget().FindByShortName("get");
CxList getsToRemove = filteredGets - originalGets;
inputs -= getsToRemove;

/*		
	Prepending some path to a file input will make it relative to the appended prefix.
	Therefore, Find_Path_Prepending_Operations results act as sanitizers in Absolute_Path_Traversal
	vulnerabilities.
*/
CxList pathPrependingOperations = Find_Path_Prepending_Operations();

/*
	Returns methods replacing both OS's file separators ("\" and "/").
*/
CxList pathTraversalSanitizers = Find_Path_Traversal_Sanitize();

CxList absolutePathTraversalSanitizers = All.NewCxList();
absolutePathTraversalSanitizers.Add(pathPrependingOperations);
absolutePathTraversalSanitizers.Add(pathTraversalSanitizers);

CxList fileOpeningStatements = Find_Files_Open();
fileOpeningStatements.Add(Find_Java7_Files_Open());

CxList vulnerabilities = inputs.InfluencingOnAndNotSanitized(fileOpeningStatements, absolutePathTraversalSanitizers);

foreach(CxList res in vulnerabilities.GetCxListByPath())
{
	CxList getSource = res.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly).FindByShortName("get");
	if(getSource.Count > 0)
	{
		CxList temp = All.FindAllReferences(getSource.GetTargetOfMembers()).GetMembersOfTarget() * loadProperties;
		result.Add(temp.ConcatenatePath(res, false));
	}
	else
	{
		result.Add(res);
	}
}

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);