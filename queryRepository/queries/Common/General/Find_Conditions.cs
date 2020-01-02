// Find conditions of if statements and loops
CxList conditions = All.NewCxList();

// Ternary Expressions
CxList ternaryExpr = Find_TernaryExpr();
foreach (CxList singleTernary in ternaryExpr)
{
	try
	{
		TernaryExpr expr = singleTernary.TryGetCSharpGraph<TernaryExpr>();
		if (expr.Test != null)
		{
			conditions.Add(expr.Test.NodeId, expr.Test);
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

// if statments
CxList ifStmt = Find_Ifs();

foreach (CxList singleIf in ifStmt)
{
	try
	{
		IfStmt stmt = singleIf.TryGetCSharpGraph<IfStmt>();
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

// conditions in loops
CxList iteration = Find_IterationStmt();
foreach (CxList iter in iteration)
{
	try
	{
		IterationStmt stmt = iter.TryGetCSharpGraph<IterationStmt>();
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