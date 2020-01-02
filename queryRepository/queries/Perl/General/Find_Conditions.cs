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
			conditions.Add(All.FindById(stmt.Condition.NodeId));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

CxList ternarys = All.FindByType(typeof(TernaryExpr));
foreach(CxList ternary in ternarys){
	try{
		TernaryExpr tern = ternary.data.GetByIndex(0) as TernaryExpr;
		if(tern != null && tern.Test != null){
			conditions.Add(All.FindById(tern.Test.NodeId));
		}
	}catch(Exception exc){
		cxLog.WriteDebugMessage(exc);
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

// When we have $_ it means we added something artificially to $_,
// so we take the right hand side of the artificial assign expression
CxList children = All.FindByFathers(conditions);
CxList _ = children.FindByShortName("$_");
CxList fathers = _.GetFathers();
conditions.Add(All.FindByFathers(fathers).FindByAssignmentSide(CxList.AssignmentSide.Right));

result = conditions;