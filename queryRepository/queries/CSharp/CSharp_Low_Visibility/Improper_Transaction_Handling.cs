CxList Commit = All.FindByName("*.Commit");
CxList Rollback = All.FindByName("*.Rollback");

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

	if(
		(        
			(
				RollbackInCatch.GetAncOfType(typeof(TryCatchFinallyStmt)) + 
				RollbackInFinally.GetAncOfType(typeof(TryCatchFinallyStmt))
			) * 
				CommitInTry.GetAncOfType(typeof(TryCatchFinallyStmt))
																	).Count == 0
		)
	{
		result.Add(cml);
	}
}