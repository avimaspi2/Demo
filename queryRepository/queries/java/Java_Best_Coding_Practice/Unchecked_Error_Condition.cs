CxList Catch = Find_Catch();
foreach(CxList curCatch in Catch)
{
	Catch ch = curCatch.TryGetCSharpGraph<Catch>();
	if(ch.Statements.Count == 0)
	{
		result.Add(ch.NodeId, ch);
	}
}