if(param.Length == 2)
{
	CxList outputs = param[1] as CxList;
	CxList inputs = param[0] as CxList;
	foreach(CxList outpt in outputs.GetCxListByPath())
	{
		
		CxList start = outpt.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
		CxList end = outpt.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
		if((start * end).Count == 0)
		{
			if((start * inputs).Count > 0)
			{
				result.Add(outpt);
			}
		}
	
	}
}