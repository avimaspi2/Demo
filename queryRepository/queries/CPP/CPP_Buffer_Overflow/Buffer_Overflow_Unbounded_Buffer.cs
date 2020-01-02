CxList methodInvokes = Find_Methods();
CxList unknownRefs = Find_Unknown_References();
CxList stringLiterals = Find_Strings();
CxList memAllocs = Find_Memory_Allocation();

// Helper delegate to compare two parameters as for their AbsValue (param1 < param2)
Func <CxList, CxList, bool> CompareExprsByAbsValLT = delegate(CxList fstParam, CxList sndParam){
	
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
	
	Expression firstParamExpr = fstParam.TryGetCSharpGraph<Expression>();
	Expression secondParamExpr = sndParam.TryGetCSharpGraph<Expression>();
	
	IntegerIntervalAbstractValue firstParamAbsValue = GetExprAbsValue(firstParamExpr);	
	IntegerIntervalAbstractValue secondParamAbsValue = GetExprAbsValue(secondParamExpr);
	
	if (firstParamAbsValue != null && secondParamAbsValue != null)
	{
		IAbstractValue absValueResult = firstParamAbsValue.LessThan(secondParamAbsValue);
		return (absValueResult is TrueAbstractValue);
	}
	
	return false;
	};

CxList inputs = Find_Unbounded_Inputs();
inputs.Add(Find_Read());
inputs.Add(Find_DB());

CxList strlenParams = All.GetParameters(Find_All_Strlen());
strlenParams.Add(unknownRefs.GetByAncs(strlenParams));

CxList memAllocParams = All.GetParameters(memAllocs);
memAllocParams.Add(unknownRefs.GetByAncs(memAllocParams));

// Buffer Overflow by using copy/concat functions without size verifications
CxList bufferOverflowFuncs = Find_BufferOverflow_CopyCat_Funcs();


// Methods that use format specifiers and could lead to Buffer Overflow (and get it)
CxList methodsWithFormat = Find_BufferOverflow_ScanPrint_Funcs(); 

foreach(CxList fmtMethod in methodsWithFormat) {
	CxList fmtMethodParams = All.GetParameters(fmtMethod, 1);

	// We only look for strings without format
	CxList fmtParam = fmtMethodParams.FindByAbstractValue(val => {	
		if (val is StringAbstractValue) { 
			Match match = Regex.Match((val as StringAbstractValue).Content, @"%(\d*)(\'|\*|\.)*(\d*)(\w)");
			cxLog.WriteDebugMessage(match.Success);
			if (!match.Success) {
				return true;
			}
		}
		return false;
		});
	if (fmtParam.Count > 0) {
		bufferOverflowFuncs.Add(fmtMethod);
	}
	
}

CxList firstParams = All.GetByAncs(All.GetParameters(bufferOverflowFuncs, 0));
CxList secondParams = All.GetByAncs(All.GetParameters(bufferOverflowFuncs, 1));

//Iterate over BO-prone functions and exclude the ones sanitized
foreach (CxList boFunc in bufferOverflowFuncs)
{
	CxList sanitized = All.NewCxList();
	
	CxList sourceParam = secondParams.GetByAncs(secondParams.GetParameters(boFunc, 1));
	CxList destinationParam = firstParams.GetByAncs(firstParams.GetParameters(boFunc, 0));
	
	CxList srcWithSize = sourceParam * unknownRefs;
	srcWithSize.Add(sourceParam * stringLiterals);
	CxList destWithSize = destinationParam * unknownRefs;
	destWithSize.Add(destinationParam * stringLiterals);
	
	//Sanitized by length-check on input
	CxList strlenSanitized = sourceParam.DataInfluencedBy(strlenParams);
	sanitized.Add(strlenSanitized);
	if (sanitized.Count > 0)
	{
		continue;
	}
	
	//Sanitized by allocation based on input
	CxList memAllocSanitized = destinationParam.DataInfluencedBy(memAllocs)
		.DataInfluencedBy(memAllocParams.FindAllReferences(secondParams));
	sanitized.Add(memAllocSanitized);
	if (sanitized.Count > 0)
	{
		continue;
	}
	
	bool srcSmallerThanDest = CompareExprsByAbsValLT(srcWithSize, destWithSize);
	if (!srcSmallerThanDest) //source parameter is not guaranteed to be smaller than destination parameter
	{
		CxList bufferOverflow = sourceParam.DataInfluencedBy(inputs);
		if (bufferOverflow.Count > 0)
		{
			CxList boWithDestination = bufferOverflow.ConcatenatePath(destWithSize, false);
			result.Add(boWithDestination);
		}
		else
		{
			bool destSmallerThanSrc = CompareExprsByAbsValLT(destWithSize, srcWithSize);
			if (destSmallerThanSrc) //destination parameter is smaller than source parameter
			{
				CxList sourceToDestination = srcWithSize.ConcatenatePath(destWithSize, false);
				result.Add(sourceToDestination);
			}
		}
	}
}

// The gets function (dangerous by default)
CxList getsFunctionParam = All.GetParameters(methodInvokes.FindByShortName("gets"));
getsFunctionParam -= getsFunctionParam.FindByType(typeof(Param));
result.Add(getsFunctionParam);