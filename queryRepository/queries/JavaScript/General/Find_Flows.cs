//receives 2 parameters, the first is influenced by X and the second is the one we want to conntect to it.
//This query connects 2 elements that supposed to have flow between them.
if(param.Length == 2)
{
	CxList reference = param[0] as CxList;
	CxList potentialVul = param[1] as CxList;	
	foreach(CxList curRef in reference)
	{
		CSharpGraph g = curRef.GetFirstGraph();		
		if(g == null || g.ShortName == null || g.LinePragma == null)
		{
			continue;
		}
		string name = g.ShortName;
		string fileName = g.LinePragma.FileName;
		CxList vul = potentialVul.FindByShortName(name).FindByFileName(fileName);		
		if(curRef.Count > 0)
		{
			result.Add(vul.ConcatenateAllSources(curRef));
		}
	}
	
}