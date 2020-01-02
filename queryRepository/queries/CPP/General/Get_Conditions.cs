// If stmt
CxList ifStmts = Find_Ifs();
CxList conditions = All.FindByFathers(ifStmts);
conditions = conditions.FindByType(typeof(Expression));

// iterations stmt
CxList iterationStmts = All.FindByType(typeof(IterationStmt));
foreach (CxList iteration in iterationStmts)
{
	IterationStmt i = iteration.data.GetByIndex(0) as IterationStmt;
	
	if (i!=null && i.Test != null)
	{
		conditions.Add(All.FindById(i.Test.NodeId));
	}
}

result = All.GetByAncs(conditions);

// assert
CxList assert = Find_Methods().FindByName("assert");
CxList assertParam = All.GetByAncs(assert);
assertParam -= assertParam.FindByShortName("assert");

result.Add(assertParam);