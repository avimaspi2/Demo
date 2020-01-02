CxList close = Find_Methods().FindByName("*.close", false);
CxList AllTrys = All.GetAncOfType(typeof(TryCatchFinallyStmt));
CxList fin = All.NewCxList();	

foreach(CxList oneTry in AllTrys)
{
	TryCatchFinallyStmt t = oneTry.data.GetByIndex(0) as TryCatchFinallyStmt;
	fin.Add(All.FindById(t.Finally.NodeId));	
}
fin = All.GetByAncs(fin);

CxList Try = close.GetAncOfType(typeof(TryCatchFinallyStmt));
foreach(CxList oneTry in Try)
{
	TryCatchFinallyStmt TryGraph = oneTry.data.GetByIndex(0) as TryCatchFinallyStmt;
	CxList curTry = All.FindById(TryGraph.Try.NodeId);
	CxList TryClose = close.GetByAncs(curTry);
	CxList AllClose = close.GetByAncs(oneTry);
	
	if( (AllClose - TryClose).Count == 0)
	{
		result.Add(TryClose);
	}	
}
 
result -= result.FindByFathers(fin);