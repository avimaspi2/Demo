CxList bFalse = Find_False_Condition();
CxList bTrue = Find_True_Condition();

// Loop over all "always false" if and loop statements, and add their relevant blocks to the dead code list
foreach (CxList t in bFalse)
{
	try
	{
		CxList cond = t.GetFathers();
		if (cond.FindByType(typeof(IfStmt)).Count > 0)
		{
			IfStmt ifStmt = cond.TryGetCSharpGraph<IfStmt>();
			result.Add(ifStmt.TrueStatements.NodeId, ifStmt.TrueStatements);
		}
		else if (cond.FindByType(typeof(IterationStmt)).Count > 0)
		{
			IterationStmt iter = cond.TryGetCSharpGraph<IterationStmt>();
			result.Add(iter.Statements.NodeId, iter.Statements);
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

// Loop over all "always true" if statements, and add their relevant blocks to the dead code list
foreach (CxList t in bTrue)
{
	try
	{
		CxList cond = t.GetFathers();
		if (cond.FindByType(typeof(IfStmt)).Count > 0)
		{
			IfStmt ifStmt = cond.TryGetCSharpGraph<IfStmt>();
			result.Add(ifStmt.FalseStatements.NodeId, ifStmt.FalseStatements);
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}