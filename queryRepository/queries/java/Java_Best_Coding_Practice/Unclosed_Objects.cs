CxList close = Find_Methods().FindByName("*.close");
CxList AllTrys = All.GetAncOfType(typeof(TryCatchFinallyStmt));

CxList fin = All.GetFinallyClause(AllTrys);
fin = All.GetByAncs(fin);

CxList Try = close.GetAncOfType(typeof(TryCatchFinallyStmt));
foreach(CxList oneTry in Try)
{
	TryCatchFinallyStmt TryGraph = oneTry.TryGetCSharpGraph<TryCatchFinallyStmt>();
	CxList curTry = All.FindById(TryGraph.Try.NodeId);
	CxList TryClose = close.GetByAncs(curTry);
	CxList AllClose = close.GetByAncs(oneTry);
	
	if( (AllClose - TryClose).Count == 0)
	{
		result.Add(TryClose);
	}	
}
 
result -= result.FindByFathers(fin);