CxList Catch = All.FindByType(typeof(Catch));
foreach(CxList curCatch in Catch)
{
	try{
		Catch ch = curCatch.data.GetByIndex(0) as Catch;
		if(ch.Statements.Count == 0)
		{
			result.Add(curCatch);
		}
	}
	catch (Exception ex){
		cxLog.WriteDebugMessage(ex);
	}
}