Func<BinaryOperator, bool> isSmallerOperator = o => o == BinaryOperator.LessThan || o == BinaryOperator.LessThanOrEqual;
Func<BinaryOperator, bool> isGreaterOperator = o => o == BinaryOperator.GreaterThan || o == BinaryOperator.GreaterThanOrEqual;
const int maxValue = 10000;

CxList inputs = Find_Interactive_Inputs();

CxList loops = Find_IterationStmt();
CxList ifStmt = Find_Ifs();
CxList conditions = Find_Conditions();
CxList loopConditions = conditions.FindByFathers(loops);
CxList ifConditions = conditions.FindByFathers(ifStmt);

inputs -= inputs.GetByAncs(loopConditions);

CxList unknown = Find_UnknownReference();
loopConditions = unknown.GetByAncs(loopConditions);

CxList rIf = loopConditions.GetAncOfType(typeof(IfStmt));
CxList rCond = ifConditions.FindByFathers(rIf);
rCond = All.GetByAncs(rCond);
CxList reff = rCond.FindAllReferences(loopConditions);
CxList loopsReff = loopConditions.FindAllReferences(reff);

loopConditions -= loopsReff.GetByAncs(rIf);

// remove cases like (currentLine != null) which is a type of loop checking
CxList nullLiteral = All.GetByAncs(conditions).FindByType(typeof(NullLiteral));
loopConditions -= loopConditions.GetByAncs(nullLiteral.GetAncOfType(typeof(BinaryExpr)));

// remove cases like (stream.read() != -1) which is a type of loop checking
CxList minusOne = All.GetByAncs(conditions).FindByType(typeof(IntegerLiteral)).FindByShortName("-1");
CxList readMethod = All.GetByAncs(conditions).FindByType("InputStream").GetRightmostMember().FindByShortName("read");
loopConditions -= loopConditions.GetByAncs((minusOne.GetAncOfType(typeof(BinaryExpr)) * readMethod.GetAncOfType(typeof(BinaryExpr))));

CxList sanitize = Unchecked_Input_for_Loop_Condition_Sanitize();
	
sanitize.Add(sanitize.GetTargetOfMembers());
sanitize.Add(Find_DB_In());
sanitize.Add(Find_Dead_Code_Contents());

// remove inputs that's values are bound
CxList comparersInLoops = All.GetByAncs(loops);
IAbstractValue upperBoundAbsIntValue = new IntegerIntervalAbstractValue(null, maxValue);
IAbstractValue lowerBoundAbsIntValue = new IntegerIntervalAbstractValue(-1 * maxValue, null);

CxList unboundLoopConditions = All.NewCxList();
foreach (CxList loopCondition in loopConditions)
{
	try
	{
		CxList comparer = loopCondition.GetFathers();
		var binaryExpression = comparer.TryGetCSharpGraph<BinaryExpr>();
		if (binaryExpression == null) 
		{
			unboundLoopConditions.Add(loopCondition);
			continue;
		}
		CxList rightSide = All.FindById(binaryExpression.Right.DomId);
		CxList leftSide = All.FindById(binaryExpression.Left.DomId);
		var binaryOperator = binaryExpression.Operator;
		
		if ((rightSide == loopCondition && isGreaterOperator(binaryOperator)) ||
			(leftSide == loopCondition && isSmallerOperator(binaryOperator)))
		{
			CxList boundValue = loopCondition.FindByAbstractValue(absInt => absInt.IncludedIn(lowerBoundAbsIntValue, true));
			if (boundValue.Count != 0) continue;
		}
		if ((rightSide == loopCondition && isSmallerOperator(binaryOperator)) ||
			(leftSide == loopCondition && isGreaterOperator(binaryOperator)))
		{
			CxList boundValue = loopCondition.FindByAbstractValue(absInt => absInt.IncludedIn(upperBoundAbsIntValue, true));
			if (boundValue.Count != 0) continue;
		}
		
		unboundLoopConditions.Add(loopCondition);
	}
	catch (Exception e)
	{
		cxLog.WriteDebugMessage(e.Message);
	}
}

result = Remove_JSP_Duplicates_Due_To_Include_Tag(unboundLoopConditions.InfluencedByAndNotSanitized(inputs, sanitize));
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);