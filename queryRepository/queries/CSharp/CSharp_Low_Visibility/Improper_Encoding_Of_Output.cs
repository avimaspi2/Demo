//This query finds Html encoding that is executed within a JavaScript block.

CxList htmlEncodes = Find_HTML_Encode();
CxList JSAll = AllMembers.All.FindByLanguage("JavaScript");

// remove razor auto-encodes
htmlEncodes -= Find_Razor_AutoEncode() + Find_Aspx_AutoEncode();

foreach (CxList curNode in htmlEncodes) 
{
	CSharpGraph g = curNode.data.GetByIndex(0) as CSharpGraph;
	if (g.LinePragma != null && g.LinePragma.FileName != null && g.LinePragma.Line != null) 
	{
		CxList inlineJavaScript = JSAll.FindByPosition(g.LinePragma.FileName, g.LinePragma.Line);
		if (inlineJavaScript.Count > 0) 
		{
			result.Add(curNode);
		}
	}
}