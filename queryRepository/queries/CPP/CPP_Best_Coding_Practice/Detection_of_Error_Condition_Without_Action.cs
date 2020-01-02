CxList Catch = Find_Catch();
foreach(CxList curCatch in Catch)
{
	Catch ch = curCatch.data.GetByIndex(0) as Catch;
	if(ch.Statements.Count == 0)
	{
		result.Add(curCatch);
	}
}