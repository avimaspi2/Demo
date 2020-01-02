CxList methods = Find_Methods();
CxList commit = methods.FindByMemberAccess("*.commit");
CxList rollback = methods.FindByMemberAccess("*.rollback");
rollback.Add(methods.FindByMemberAccess("*.abortUnlessCommitted"));

CxList TryBlock = commit.GetAncOfType(typeof(TryCatchFinallyStmt));
foreach(CxList cml in TryBlock)
{
	TryCatchFinallyStmt TryGraph = cml.TryGetCSharpGraph<TryCatchFinallyStmt>();
	
	CxList curTry = All.FindById(TryGraph.Try.NodeId);

	
	CxList curCatch = All.NewCxList();
	if(TryGraph.CatchClauses != null)
	{
		foreach (Catch clause in TryGraph.CatchClauses)
		{
			curCatch.Add(clause.NodeId, clause);
		}
	}

	CxList curFinally = All.NewCxList();
	if(TryGraph.CatchClauses != null && TryGraph.Finally.Count > 0)
	{
		curFinally = All.FindById(TryGraph.Finally.NodeId);
	}

	CxList CommitInTry = commit.GetByAncs(curTry);

	CxList RollbackInCatch = rollback.GetByAncs(curCatch);
	CxList RollbackInFinally = rollback.GetByAncs(curFinally);
	CxList goodTryStmt = RollbackInCatch.GetAncOfType(typeof(TryCatchFinallyStmt));
	goodTryStmt.Add(RollbackInFinally.GetAncOfType(typeof(TryCatchFinallyStmt)));
	// Some rollback methods could throw an exception and so should be wrapped in another try
	goodTryStmt.Add(goodTryStmt.GetFathers().GetAncOfType(typeof(TryCatchFinallyStmt)));
	
	CxList testedTry = CommitInTry.GetAncOfType(typeof(TryCatchFinallyStmt)); 
	
	if((goodTryStmt * testedTry).Count == 0)
	{
		result.Add(TryGraph.NodeId, TryGraph);
	}	
	
}