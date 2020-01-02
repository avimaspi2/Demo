// Find conditions variables of if statements
CxList ifStmt = All.FindByType(typeof(IfStmt));
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

CxList conditionsVars = All.GetByAncs(conditions);

result.Add(All.FindAllReferences(conditionsVars));