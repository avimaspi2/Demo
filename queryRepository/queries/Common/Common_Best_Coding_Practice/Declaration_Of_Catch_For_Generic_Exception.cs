List<string> genericExceptionTypes = param.Length == 1 ?
	param[0] as List<string> :
	new List<string>();

CxList Try = All.FindByType(typeof(TryCatchFinallyStmt));

foreach(CxList curTry in Try)
{
	try
	{
		TryCatchFinallyStmt tr = curTry.data.GetByIndex(0) as TryCatchFinallyStmt;
		if(tr.CatchClauses.Count == 1)
		{
			var exceptionType = tr.CatchClauses[0].CatchExceptionType;
			if (exceptionType == null || genericExceptionTypes.Contains(exceptionType.ShortName))
				result.Add(All.FindById(tr.CatchClauses[0].NodeId));
		}
	}
	catch (Exception e)
	{
		cxLog.WriteDebugMessage(e.Message);
	}
}