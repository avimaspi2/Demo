// this query used to remove “original” results from heuristic results.
// if in results appears path that its source exists in source CxList and
// its destination exists in destination CxList -> this path shall NOT appears in final result

if (param.Length == 3)
{
	CxList orgResult = param[0] as CxList; // temporary heuristic results
	CxList source = param[1] as CxList;    // Source CxList
	CxList target = param[2] as CxList;    // destination CxList

	CxList sourceStart = source.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	CxList targetEnd = target.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

	foreach (CxList onePath in orgResult.GetCxListByPath())
	{
		CxList firstNode = onePath.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
		CxList endNode = onePath.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

		CxList containFirst = firstNode * sourceStart;
		CxList containEnd = endNode * targetEnd;
		
		if ((containFirst.Count == 0) || (containEnd.Count == 0))
		{
			result.Add(onePath);
		}
	}
}