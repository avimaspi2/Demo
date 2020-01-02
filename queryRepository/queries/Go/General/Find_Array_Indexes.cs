/* Find all accesses to array or slice positions*/

CxList arraysOrSlices = Find_IndexerRefs();

CxList arrayAccesses = All.NewCxList();
foreach(CxList arrayOrSlice in arraysOrSlices)
{
	IndexerRef idxRef = arrayOrSlice.TryGetCSharpGraph<IndexerRef>();
	foreach (var expr in idxRef.Indices)
	{
		if (expr is CSharpGraph)
		{
			arrayAccesses.Add(expr.NodeId, expr);
		}
	}
}

result = arrayAccesses;