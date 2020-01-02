// Find conditions of if statements and loops

CxList ifStmt = Find_Ifs();
CxList iteration = Find_IterationStmt();

CxList conditions = All.NewCxList();
foreach (CxList singleIf in ifStmt)
{
	try
	{
		IfStmt stmt = singleIf.TryGetCSharpGraph<IfStmt>();
		if (stmt.Condition != null)
		{
			conditions.Add(All.FindById(stmt.Condition.NodeId));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

CxList ternaryExpr = Find_TernaryExpr();
foreach (CxList singleTernary in ternaryExpr)
{
	try
	{
		TernaryExpr ternary = singleTernary.TryGetCSharpGraph<TernaryExpr>();
		if (ternary.Test != null)
		{
			conditions.Add(All.FindById(ternary.Test.NodeId));
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
			conditions.Add(All.FindById(stmt.Test.NodeId));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

result = conditions;

// assert
CxList assert = Find_Methods().FindByName("assert");
CxList assertParam = All.GetParameters(assert);
assertParam -= assertParam.FindByShortName("assert");

result.Add(assertParam);