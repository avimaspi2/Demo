if(param.Length > 0)
{
	CxList variable = param[0] as CxList;
	CxList cd = Find_Classes();
	foreach(CxList v in variable)
	{
		try{
			CSharpGraph g = v.GetFirstGraph();
			if(g != null && g.LinePragma != null && g.ShortName != null)
			{
				result.Add(cd.FindByFileId(g.LinePragma.GetFileId()).FindByType(g.TypeName.Replace("var", "")));
			}
		}catch(Exception e)
		{
			cxLog.WriteDebugMessage(e);
		}
	}
}