/* This Query will return all the conditions used in loops */
CxList loops = Find_IterationStmt();
CxList conditions = All.NewCxList();

foreach (CxList iter in loops)
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