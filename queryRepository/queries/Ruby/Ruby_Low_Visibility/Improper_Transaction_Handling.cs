CxList notStrings = All - Find_Strings();
notStrings -= notStrings.FindByType(typeof(MethodDecl));
CxList Commit = notStrings.FindByShortName("commit*", false);
Commit -= Commit.FindByType(typeof(Param));
Commit -= Commit.FindByType(typeof(UnknownReference));
CxList Rollback = notStrings.FindByShortName("rollback*", false);
Rollback -= Rollback.FindByType(typeof(Param));
Rollback -= Rollback.FindByType(typeof(UnknownReference));

CxList TryBlock = Commit.GetAncOfType(typeof(TryCatchFinallyStmt));

result = Commit - Commit.GetByAncs(TryBlock);
result -= result.FindByType(typeof(MethodRef));

foreach(CxList cml in TryBlock)
{
	TryCatchFinallyStmt TryGraph = cml.data.GetByIndex(0) as TryCatchFinallyStmt;
	CxList curTry = All.FindById(TryGraph.Try.NodeId);
	
	CxList curCatch = All.NewCxList();
	if(TryGraph.CatchClauses != null && TryGraph.CatchClauses.Count > 0)
	{
		curCatch = All.FindById(TryGraph.CatchClauses[0].NodeId);
	}
	
	CxList CommitInTry = Commit.GetByAncs(curTry);
	CxList RollbackInCatch = Rollback.GetByAncs(curCatch);


	if( (RollbackInCatch.GetAncOfType(typeof(TryCatchFinallyStmt)) * 
		CommitInTry.GetAncOfType(typeof(TryCatchFinallyStmt))).Count == 0)
	{
		result.Add(cml);
	}
}