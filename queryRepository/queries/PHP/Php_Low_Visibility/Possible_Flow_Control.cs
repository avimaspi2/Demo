CxList inputs = Find_Inputs();
inputs.Add(Find_Read());
inputs.Add(Find_File_Read());

CxList sanitized = Find_Flow_Control_Sanitize();

CxList flowClauses = All.NewCxList();
try 
{
	foreach( CxList statement in All.FindByType(typeof(SwitchStmt)) )
	{
		SwitchStmt g = statement.data.GetByIndex(0) as SwitchStmt;
		flowClauses.Add(All.FindById(g.Condition.NodeId));
	}
}
catch (Exception ex)
{
	cxLog.WriteDebugMessage(ex);
}
try 
{
	foreach( CxList statement in All.FindByType(typeof(IterationStmt)) )
	{
		IterationStmt g = statement.data.GetByIndex(0) as IterationStmt;
		if (g.Test != null)
			flowClauses.Add(All.FindById(g.Test.NodeId));	
	}
}
catch (Exception ex)
{
	cxLog.WriteDebugMessage(ex);
}
try 
{
	foreach( CxList statement in All.FindByType(typeof(IfStmt)) )
	{
		IfStmt g = statement.data.GetByIndex(0) as IfStmt;
		flowClauses.Add(All.FindById(g.Condition.NodeId));

	}
}
catch (Exception ex)
{
	cxLog.WriteDebugMessage(ex);
}
try 
{
	
	foreach( CxList statement in All.FindByType(typeof(TernaryExpr)) )
	{
		TernaryExpr g = statement.data.GetByIndex(0) as TernaryExpr;
		flowClauses.Add(All.FindById(g.Test.NodeId));
	}
}
catch (Exception ex)
{
	cxLog.WriteDebugMessage(ex);
}

result = flowClauses.InfluencedByAndNotSanitized(inputs, sanitized);