// Find conditions of if statements and loops

CxList ifStmt = All.FindByType(typeof(IfStmt));
CxList iteration = All.FindByType(typeof(IterationStmt));

CxList conditions = All.NewCxList();
foreach (CxList singleIf in ifStmt)
{
	try
	{
		IfStmt stmt = singleIf.data.GetByIndex(0) as IfStmt;
		if (stmt.Condition != null)
		{
			conditions.Add(stmt.Condition.NodeId, stmt.Condition);
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

foreach (CxList iter in iteration)
{
	try
	{
		IterationStmt stmt = iter.data.GetByIndex(0) as IterationStmt;
		if (stmt.Test != null)
		{
			conditions.Add(stmt.Test.NodeId, stmt.Test);
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

result = conditions;