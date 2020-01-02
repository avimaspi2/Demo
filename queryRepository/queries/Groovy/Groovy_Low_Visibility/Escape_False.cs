CxList jspTags = Find_Output_Tags();
CxList property = jspTags.FindByMemberAccess("_property.*");
CxList escape = (Find_Jsp_Code() * Find_Methods());

CxList noEscape = Find_Jsp_Code().FindByMemberAccess("cx_escFalse.*");


// Remove double results, coming for multiple-scan of jsp files:
ArrayList lines = new ArrayList();
foreach(CxList esc in noEscape)
{
	CSharpGraph fromGraph = (CSharpGraph) esc.data.GetByIndex(0);
	string key = fromGraph.LinePragma.FileName + "_" + fromGraph.LinePragma.Line;
	if (!lines.Contains(key))
	{
		result.Add(fromGraph.NodeId, fromGraph);
		lines.Add(key);
	}
}