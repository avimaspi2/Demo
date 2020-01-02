CxList Try = All.FindByType(typeof(TryCatchFinallyStmt));
CxList Catch = All.FindByType(typeof(Catch));
CxList generalException = All.FindByName("Exception").GetFathers().FindByType(typeof(Catch));

CxList genExc = All.FindAllReferences(Catch); // an exception type was found

generalException = Catch - All.FindAllReferences(Catch);

foreach(CxList curTry in Try)
{
	TryCatchFinallyStmt tr = curTry.data.GetByIndex(0) as TryCatchFinallyStmt;
	try
	{
		if (tr.CatchClauses != null && tr.CatchClauses.Count == 1)
		{
			if (generalException.data.Contains(tr.CatchClauses[0].NodeId))
			{
				result.Add(All.FindById(tr.CatchClauses[0].NodeId));
			}
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}