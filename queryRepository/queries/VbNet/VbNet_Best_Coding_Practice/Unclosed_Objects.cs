CxList close = Find_Methods().FindByName("*.Close", false);
CxList AllTrys = All.GetAncOfType(typeof(TryCatchFinallyStmt));
CxList fin = All.NewCxList();	

foreach(CxList oneTry in AllTrys)
{
	try{
		TryCatchFinallyStmt t = oneTry.data.GetByIndex(0) as TryCatchFinallyStmt;
		fin.Add(All.FindById(t.Finally.NodeId));
	}
	catch (Exception ex){
		cxLog.WriteDebugMessage(ex);
	}
}
fin = All.GetByAncs(fin);

CxList Try = close.GetAncOfType(typeof(TryCatchFinallyStmt));
foreach(CxList oneTry in Try)
{
	try{
		TryCatchFinallyStmt TryGraph = oneTry.data.GetByIndex(0) as TryCatchFinallyStmt;
		CxList curTry = All.FindById(TryGraph.Try.NodeId);
		CxList TryClose = close.GetByAncs(curTry);
		CxList AllClose = close.GetByAncs(oneTry);
	
		if( (AllClose - TryClose).Count == 0)
		{
			if (TryClose.GetAncOfType(typeof(UsingStmt)).Count == 0)
			{
				result.Add(TryClose);
			}
		}
	}
	catch (Exception ex){
		cxLog.WriteDebugMessage(ex);
	}
}
 
result -= result.FindByFathers(fin);