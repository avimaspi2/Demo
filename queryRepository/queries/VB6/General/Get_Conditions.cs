// If stmt
CxList ifStmts = All.FindByType(typeof(IfStmt));
CxList conditions = All.FindByFathers(ifStmts);
conditions = conditions.FindByType(typeof(Expression));

// iterations stmt
CxList iterationStmts = All.FindByType(typeof(IterationStmt));
foreach (CxList iteration in iterationStmts)
{
	IterationStmt i = iteration.data.GetByIndex(0) as IterationStmt;
	if (i.Test != null)
	{
		conditions += All.FindById(i.Test.NodeId);
	}
}

result = All.GetByAncs(conditions);