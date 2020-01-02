// If stmt
CxList ifStmts = Find_Ifs();
CxList conditions = All.FindByFathers(ifStmts);
conditions = conditions.FindByType(typeof(Expression));

// iterations stmt
CxList iterationStmts = Find_IterationStmt();
foreach (CxList iteration in iterationStmts)
{
	IterationStmt i = iteration.TryGetCSharpGraph<IterationStmt>();
	if (i.Test != null)
	{
		conditions.Add(All.FindById(i.Test.NodeId));
	}
}

result = All.GetByAncs(conditions);

// assert
CxList assert = Find_Methods().FindByShortName("*assert", false);
CxList assertParam = All.GetByAncs(assert);
assertParam -= assertParam.FindByShortName("*assert", false);

result.Add(assertParam);