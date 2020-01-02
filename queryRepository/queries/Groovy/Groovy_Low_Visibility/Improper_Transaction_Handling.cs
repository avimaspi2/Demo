CxList Commit = All.FindByName("*.commit");
CxList Rollback = All.FindByName("*.rollback");

CxList TryBlock = Commit.GetAncOfType(typeof(TryCatchFinallyStmt));
foreach(CxList cml in TryBlock)
{
	TryCatchFinallyStmt TryGraph = cml.data.GetByIndex(0) as TryCatchFinallyStmt;
	
	CxList curTry = All.FindById(TryGraph.Try.NodeId);

	
	CxList curCatch = All.NewCxList();
	if(TryGraph.CatchClauses != null && TryGraph.CatchClauses.Count > 0)
	{
		curCatch = All.FindById(TryGraph.CatchClauses[0].NodeId);
	}
	
	CxList curFinally = All.NewCxList();
	if(TryGraph.CatchClauses != null && TryGraph.Finally.Count > 0)
	{
		curFinally = All.FindById(TryGraph.Finally.NodeId);
	}
	
	CxList CommitInTry = Commit.GetByAncs(curTry);

	CxList RollbackInCatch = Rollback.GetByAncs(curCatch);
	CxList RollbackInFinally = Rollback.GetByAncs(curFinally);
	CxList goodTryStmt = 
		RollbackInCatch.GetAncOfType(typeof(TryCatchFinallyStmt)) + 
		RollbackInFinally.GetAncOfType(typeof(TryCatchFinallyStmt));
	goodTryStmt = goodTryStmt.GetFathers().GetAncOfType(typeof(TryCatchFinallyStmt));
	
	CxList testedTry = CommitInTry.GetAncOfType(typeof(TryCatchFinallyStmt)); 
	
	if((goodTryStmt * testedTry).Count == 0)
	{
		result.Add(TryGraph.NodeId, TryGraph);
	}	
	
}