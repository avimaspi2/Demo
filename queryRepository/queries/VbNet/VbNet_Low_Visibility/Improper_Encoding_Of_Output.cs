//This query finds Html encoding that is executed within a JavaScript block.
CxList htmlEncodes = Find_HTML_Encode();
CxList JSAll = AllMembers.All.FindByLanguage("JavaScript");

CxList heuristicSanitizers = Find_Methods().
	FindByShortNames(new List<string> {"*encodeForJS*", "*jsEncode*", "*EncodeJS*"}, false);
CxList toRemove = htmlEncodes
	.DataInfluencingOn(heuristicSanitizers)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
htmlEncodes -= toRemove;

foreach (CxList curNode in htmlEncodes) 
{
	CSharpGraph g = curNode.data.GetByIndex(0) as CSharpGraph;
	if (g != null && g.LinePragma != null && g.LinePragma.FileName != null && g.LinePragma.Line != null) 
	{
		CxList inlineJavaScript = JSAll.FindByPosition(g.LinePragma.FileName, g.LinePragma.Line);
		if (inlineJavaScript.Count > 0) 
		{
			result.Add(curNode);
		}
	}
}