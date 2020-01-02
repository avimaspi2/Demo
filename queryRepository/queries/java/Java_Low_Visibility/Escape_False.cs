CxList jspTags = Find_Output_Tags();
CxList jspCode = Find_Jsp_Code();


CxList property = jspTags.FindByMemberAccess("_property.*");
CxList escape = (jspCode * Find_Methods());

CxList noEscape = jspCode.FindByMemberAccess("cx_escFalse.*");


// Remove double results, coming for multiple-scan of jsp files:
ArrayList lines = new ArrayList();
foreach(CxList esc in noEscape)
{
	CSharpGraph fromGraph = esc.TryGetCSharpGraph<CSharpGraph>();;
	string key = fromGraph.LinePragma.FileName + "_" + fromGraph.LinePragma.Line;
	if (!lines.Contains(key))
	{
		result.Add(fromGraph.NodeId, fromGraph);
		lines.Add(key);
	}
}