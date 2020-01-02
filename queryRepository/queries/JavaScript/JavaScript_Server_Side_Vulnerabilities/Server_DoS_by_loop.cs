IAbstractValue limit = new IntegerIntervalAbstractValue(-10000, 10000);
Func<IAbstractValue, bool> IsLimitedValue = abstractValue => abstractValue.IncludedIn(limit, true);

// Find all loops, who influenced by User Input, so User can send BIG number to For loop, causing DoS
CxList inputs = NodeJS_Find_Interactive_Inputs();

CxList iterationStatement = Find_Loops();
CxList tests = All.NewCxList();
foreach (CxList pair in iterationStatement)
{
	IterationStmt stmt = pair.TryGetCSharpGraph<IterationStmt>();
	if (stmt != null && stmt.Test != null)
	{
		Expression test = stmt.Test;
		tests.Add(test.NodeId, test);
	}
}
//find BinaryExpr in conditions of Loops
CxList binaryExpr = tests * Find_Binarys();

//Find elements of conditions
CxList ChildOfBinary = All.GetByAncs(binaryExpr);
// Remove all the children who's range are limmited in both directions, and their brothers
CxList sanitizers = All.NewCxList();
CxList limitedChildOfBinary = All.NewCxList();
foreach (CxList child in ChildOfBinary){
	CxList brothers = ChildOfBinary.FindByFathers(child.GetFathers());
	if (brothers.FindByAbstractValue(IsLimitedValue).Count == 0)
		continue;
	limitedChildOfBinary.Add(brothers);
}
sanitizers.Add(limitedChildOfBinary);

//If input is checked (not inside of loop's conditions) - it's OK
//Find sanitasion by ifStmt and TernaryExpr
CxList ifStmt = Find_Ifs();
CxList ternary = Find_TernaryExpr();

CxList conditions = All.NewCxList();

// build ifCond list: the list of the all conditions
foreach(CxList ifst in ifStmt)
{
	try{
		IfStmt stmt = ifst.TryGetCSharpGraph<IfStmt>();
		if(stmt != null && stmt.Condition != null)
		{
			Expression condition = stmt.Condition;
			conditions.Add(condition.NodeId, condition);		
		}
	} catch(Exception e){
		cxLog.WriteDebugMessage(e);
	}
}

//build terCon list: the list of all Ternary
foreach(CxList ifst in ternary)
{
	try{
		TernaryExpr stmt = ifst.TryGetCSharpGraph<TernaryExpr>();
		if(stmt != null && stmt.Test != null)
		{
			Expression condition = stmt.Test;
			conditions.Add(condition.NodeId, condition);		
		}
	} catch(Exception e){
		cxLog.WriteDebugMessage(e);
	}
}

//Build lessSide list to the objests in the Less operator size
//Build greatSide list to the objects in the Great operator size  
CxList lessSide = All.NewCxList();
CxList greatSide = All.NewCxList();
foreach (CxList cml in conditions)
{
	try{
		BinaryExpr graph = cml.TryGetCSharpGraph<BinaryExpr>();
		Expression Less = null;
		Expression Great = null;
		if (graph != null)
		{
			if (graph.Left != null && graph.Right != null )
			{
				if (graph.Operator == BinaryOperator.LessThan || graph.Operator == BinaryOperator.LessThanOrEqual)
				{
					Less = graph.Left;
					Great = graph.Right;
				}
				else if (graph.Operator == BinaryOperator.GreaterThan || graph.Operator == BinaryOperator.GreaterThanOrEqual)
				{
					Less = graph.Right;
					Great = graph.Left;
				}
				if (Less != null && Great != null)
				{
					lessSide.Add(Less.NodeId, Less);
					greatSide.Add(Great.NodeId, Great);
				}
			}
		}
	} catch(Exception e){
		cxLog.WriteDebugMessage(e);
	}
}

CxList lessInfluenced = lessSide.DataInfluencedBy(inputs);
CxList greatInfluenced = greatSide.DataInfluencedBy(inputs);
// run on the all lessInfluenced and build sanitizers
foreach (CxList cml in lessInfluenced)
{
	try{
		IGraph graph = cml.GetFirstGraph();
		CSharpGraph cs = graph as CSharpGraph;
		if (cs == null)
		{
			continue;
		}
		while (graph != null && !(graph is IfStmt) && !(graph is TernaryExpr))
		{
			graph = graph._father;
		}
		if (graph != null)
		{
			CxList trueStmt = All.NewCxList();
			if (graph is IfStmt)
			{
				IfStmt target = graph as IfStmt;
				trueStmt = All.FindById(target.TrueStatements.NodeId);
			}
			else
			{
				TernaryExpr target = graph as TernaryExpr;
				trueStmt = All.FindById(target.True.NodeId);
			}
			CxList reference = All.GetByAncs(trueStmt);
			CxList source = All.FindById(cs.NodeId);
			sanitizers.Add(reference.FindAllReferences(source));
		}
	} catch(Exception e){
		cxLog.WriteDebugMessage(e);
	}
}

//run on the all greatInfluenced and build sanitizers
foreach (CxList cml in greatInfluenced)
{
	try{
		IGraph graph = cml.GetFirstGraph();
		CSharpGraph cs = graph as CSharpGraph;
		if (cs == null)
		{
			continue;
		}
		while (graph != null && !(graph is IfStmt) && !(graph is TernaryExpr))
		{
			graph = graph._father;
		}
		if (graph != null)
		{
			CxList falseStmt = All.NewCxList();
			if (graph is IfStmt)
			{
				IfStmt target = graph as IfStmt;
				falseStmt = All.FindById(target.FalseStatements.NodeId);
			}
			else
			{
				TernaryExpr target = graph as TernaryExpr;
				falseStmt = All.FindById(target.False.NodeId);
			}
			CxList reference = All.GetByAncs(falseStmt);
			CxList source = All.FindById(cs.NodeId);
			sanitizers.Add(reference.FindAllReferences(source));
		}
	} catch(Exception e){
		cxLog.WriteDebugMessage(e);
	}
}
 
CxList sanitizerDB = NodeJS_Find_DB_Base();
sanitizers.Add(sanitizerDB);

// return this, who influenced by Input and not sanitized
result = ChildOfBinary.InfluencedByAndNotSanitized(inputs, sanitizers);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);