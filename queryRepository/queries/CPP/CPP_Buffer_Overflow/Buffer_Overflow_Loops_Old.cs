//	Buffer_Overflow - loops
//  -----------------
//  Find all the loops that have "<=" instead of "<", thus may create
//  Buffer Overflow problem.
///////////////////////////////////////////////////////////////////////

// Find all iterations/loops
CxList iterations = Find_IterationStmt();
// Keep all iterations members (for performance, not to use All anymore)
CxList allIterationsMembers = All.GetByAncs(iterations);

CxList arrayCreate = Find_ArrayCreateExpr();
CxList arraySize = Find_Integer_Literals().GetByAncs(arrayCreate);

// Run on every loop and look for the vulnerability
foreach (CxList iteration in iterations)
{
	// Check if we are dealing with a potential problem ("<=")
	IterationStmt iter = iteration.data.GetByIndex(0) as IterationStmt;

	if ((iter.Test != null) && (iter.Test.ShortName.Equals("<=")))
	{
		// Keep all the members of the current iteration (performance)
		CxList iterationMembers = allIterationsMembers.GetByAncs(iteration);

		// Find all initializing expression of the iteration
		CxList initExpr = All.NewCxList();
		foreach (Statement stmt in iter.Init)
		{
			initExpr.Add(iterationMembers.FindById(stmt.NodeId));
		}

		CxList condition = All.FindById(iter.Test.NodeId);
		CxList indexUses = iterationMembers.FindAllReferences(iterationMembers.GetByAncs(initExpr).FindByAssignmentSide(CxList.AssignmentSide.Left));
		CxList arrays = iterationMembers.FindByType(typeof(IndexerRef));
		CxList sizes = arraySize.GetByAncs(All.FindDefinition(arrays));
		CxList conditionParts = All.GetByAncs(condition) - condition - indexUses;

		// Sanitize False Positive where Loop Condition < Array Size
		bool initLikeCondition = true;
		foreach (CxList size in sizes)
		{
			IntegerLiteral intSize = size.data.GetByIndex(0) as IntegerLiteral;
			long intArraySize = intSize.Value;

			long intLoopCond = 0; 
			if (conditionParts.FindByType(typeof(IntegerLiteral)).Count >= 1)
			{
				IntegerLiteral intCond = conditionParts.data.GetByIndex(0) as IntegerLiteral;
				if (intCond != null)
				{
					intLoopCond = intCond.Value;
				}
			}

			if (intLoopCond < intArraySize)
			{
				initLikeCondition = false;
			}
		}

		indexUses = indexUses.FindByFathers(arrays);
		// Find the values of initialization
		CxList val = iterationMembers.GetByAncs(initExpr).FindByAssignmentSide(CxList.AssignmentSide.Right);
		// If exists a value 0, then it's probably a problematic loop.
		if (initLikeCondition && (val.FindByShortName("0").Count > 0) && (indexUses.Count > 0))
		{
			CxList iterationValue = iterationMembers.FindById(iter.Test.NodeId);
			CxList sizeMethod = conditionParts.FindByType(typeof(MethodInvokeExpr)).FindByShortName("size");
			if ((sizeMethod - iterationMembers.FindById(iter.Test.NodeId)).Count > 0)
			{
				CxList binary = conditionParts.FindByType(typeof(BinaryExpr));
				if (iterationMembers.GetByAncs(binary).Count > 0)
				{
					iterationValue -= iterationValue;
				}
			}
			result.Add(iterationValue);
		}

		// Sanitize if minus in interation test
		CxList TestNode = iterationMembers.FindById(iter.Test.NodeId);
		BinaryExpr b = conditionParts.data.GetByIndex(0) as BinaryExpr;
		if (b != null && b.Operator == BinaryOperator.Subtract)
		{
			result -= TestNode;
		}

	}
}