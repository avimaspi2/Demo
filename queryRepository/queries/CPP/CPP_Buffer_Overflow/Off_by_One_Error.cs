CxList methods = Find_Methods();
CxList binaryExprs = Find_BinaryExpressions();
CxList iterations = Find_Iterations();
CxList unknRefs = Find_Unknown_References();
CxList arrays = Find_Arrays();

CxList subtractions = binaryExprs.GetByBinaryOperator(BinaryOperator.Subtract);
//heuristic part
unknRefs -= unknRefs.InfluencedBy(subtractions);

//Loops
CxList iterationsConditions = binaryExprs.GetByAncs(iterations);
CxList dangerousIterationsConditions = All.NewCxList();
dangerousIterationsConditions.Add(iterationsConditions.GetByBinaryOperator(BinaryOperator.GreaterThanOrEqual));
dangerousIterationsConditions.Add(iterationsConditions.GetByBinaryOperator(BinaryOperator.LessThanOrEqual));

CxList dangerousIterations = dangerousIterationsConditions.GetAncOfType(typeof(IterationStmt));
foreach(CxList iteration in dangerousIterations)
{
	IterationStmt iterStmt = iteration.TryGetCSharpGraph<IterationStmt>();
	if(iterStmt != null && iterStmt.Test != null)
	{
		CxList condition = dangerousIterationsConditions.FindById(iterStmt.Test.NodeId);
		BinaryExpr expr = condition.TryGetCSharpGraph<BinaryExpr>();
		if(expr != null && expr.Right != null && !subtractions.ContainsKey(expr.Right.NodeId))
		{
			CxList arraysInBlock = arrays.GetByAncs(iteration);
			CxList iterator = unknRefs.FindById(expr.Left.NodeId);		
			CxList iteratorRefs = unknRefs.FindAllReferences(iterator).GetByAncs(iteration);
			CxList iteratorUsedInArray = iteratorRefs.GetByAncs(arraysInBlock);			
			iteratorUsedInArray -= iteratorUsedInArray.GetByAncs(subtractions);
			foreach(CxList iterInArray in iteratorUsedInArray)
			{
				result.Add(iterator.Concatenate(iterInArray, false));
			}
		}
	}
}

//Arrays
CxList sizeOf = methods.FindByShortName("sizeof");
sizeOf -= sizeOf.GetByAncs(subtractions);
CxList arraysAccessesWithSizeOf = sizeOf.GetByAncs(arrays).GetAncOfType(typeof(IndexerRef));
result.Add(arraysAccessesWithSizeOf);

result.Add(Find_Improper_Index_Access(true));

//Methods
//Methods that do not check boundaries
CxList copyMethodsWithSize = methods.FindByShortName("readlink");
copyMethodsWithSize.Add(Find_All_strncpy());
copyMethodsWithSize.Add(Find_All_strncat());
result.Add(sizeOf.GetParameters(copyMethodsWithSize));

//Methods that copy without checking \0
CxList copyMethods = methods.FindByShortNames(new List<string>{"strcpy_s", "strcat", "strcpy"});

//Helper delegate to obtain expression's abstract value
Func<Expression, IntegerIntervalAbstractValue> GetExprAbsValue = delegate(Expression paramExpr) {
	IntegerIntervalAbstractValue paramAbsValue = null;
	if(paramExpr != null)
	{
		IAbstractValue absValue = paramExpr.AbsValue;
		if (absValue is ObjectAbstractValue)
		{
			paramAbsValue = (absValue as ObjectAbstractValue).AllocatedSize;
		}
		else if (absValue is IntegerIntervalAbstractValue)
		{
			paramAbsValue = absValue as IntegerIntervalAbstractValue;
		}
		else if (absValue is StringAbstractValue)
		{
			StringAbstractValue stringAbsValue = absValue as StringAbstractValue;
			paramAbsValue = new IntegerIntervalAbstractValue(stringAbsValue.Content.Length);
		}
	}
	return paramAbsValue;
};

foreach(CxList method in copyMethods)
{
	CxList firstParam = unknRefs.GetParameters(method, 0);
	CxList secondParam = unknRefs.GetParameters(method, 1);
	
	Expression firstParamExpr = firstParam.TryGetCSharpGraph<Expression>();
	Expression secondParamExpr = secondParam.TryGetCSharpGraph<Expression>();
	
	IntegerIntervalAbstractValue firstParamAbsValue = GetExprAbsValue(firstParamExpr);	
	IntegerIntervalAbstractValue secondParamAbsValue = GetExprAbsValue(secondParamExpr);
	
	if (firstParamAbsValue != null && secondParamAbsValue != null)
	{
		IAbstractValue absValueResult = firstParamAbsValue.LessThanOrEqual(secondParamAbsValue);
		if(absValueResult is TrueAbstractValue)
		{
			result.Add(method);
		}
	}
}