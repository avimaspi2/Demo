CxList methodInvokes = Find_Methods();
CxList parameters = Find_Parameters();
CxList unknownRefs = Find_Unknown_References();
CxList integers = Find_Integers();
integers.Add(Find_Integer_Literals());

CxList nodesWithAbsVal = All.NewCxList();
nodesWithAbsVal.Add(unknownRefs);
nodesWithAbsVal.Add(Find_Strings());
nodesWithAbsVal.Add(Find_CharLiteral());

// Helper delegate to compare two parameters as for their AbsValue
Func <CxList, string, CxList, bool> CompareExprsByAbsVal = delegate(CxList fstParam, string compareOp, CxList sndParam){
	
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
		IAbstractValue absValueResult = new FalseAbstractValue();
		switch (compareOp)
		{
			case ">":
				absValueResult = firstParamAbsValue.GreaterThan(secondParamAbsValue);
				break;
			case "<":
				absValueResult = firstParamAbsValue.LessThan(secondParamAbsValue);
				break;
			case "==":
				absValueResult = firstParamAbsValue.IdentityEquality(secondParamAbsValue);
				break;
		}
		return (absValueResult is TrueAbstractValue);
	}
	
	return false;
};

// Inputs
CxList inputs = Find_Unbounded_Inputs();
inputs.Add(Find_Read());
inputs.Add(Find_DB());

// Sanitizers
CxList sizeofMethods = methodInvokes.FindByShortName("sizeof");
CxList sizeofParams = parameters.GetParameters(sizeofMethods);
sizeofParams.Add(unknownRefs.GetByAncs(sizeofParams));

CxList strlenMethods = Find_All_Strlen();
CxList strlenParams = parameters.GetParameters(strlenMethods);
strlenParams.Add(unknownRefs.GetByAncs(strlenParams));

CxList memAllocs = Find_Memory_Allocation();
memAllocs.Add(Find_Memory_Allocation_Dynamic());
CxList memAllocParams = parameters.GetParameters(memAllocs);
memAllocParams.Add(unknownRefs.GetByAncs(memAllocParams));

// Vulnerable Functions
List<string> srcSndParamSizeThrParam = new List<string> { "_mbsncat", "_mbsncat_l", "memcpy", "memmove", "memset",
		"strncat", "strncpy", "_strncpy_l", "strxfrm", "_tcsncat", "_tcsncat_l", "_tcsncpy", "_tcsncpy_l",
		"wcsncat", "wcsncpy", "_wcsncpy_l", "wcsxfrm", "wmemcpy", "wmemmove", "wmemset" };
List<string> srcSndParamSizeFthParam = new List<string> { "_memccpy" };
List<string> srcThrParamSizeSndParam = new List<string> { "fgets", "fgetws", "generate_n" };
List<string> srcFthParamSizeSndParam = new List<string> { "strftime", "wcsftime", "snprintf", 
		"swprintf", "vsnprintf", "vswprintf" };

List<string> vulnerableMethodsNames = new List<string>();
vulnerableMethodsNames.AddRange(srcSndParamSizeThrParam);
vulnerableMethodsNames.AddRange(srcSndParamSizeFthParam);
vulnerableMethodsNames.AddRange(srcThrParamSizeSndParam);
vulnerableMethodsNames.AddRange(srcFthParamSizeSndParam);

CxList vulnerableMethods = All.FindByShortNames(vulnerableMethodsNames);

CxList sizeParameters = All.NewCxList();
sizeParameters.Add(sizeofParams);
sizeParameters.Add(strlenParams);

CxList sizeWithValue = All.NewCxList();
sizeWithValue.Add(nodesWithAbsVal);
sizeWithValue.Add(integers);

foreach(CxList method in vulnerableMethods)
{		
	MethodInvokeExpr methodGraph = method.TryGetCSharpGraph<MethodInvokeExpr>();
	if (methodGraph != null)
	{
		string methodName = methodGraph.ShortName;
	
		// Get the source, destination and size limit parameters of each function
		CxList destination = parameters.GetParameters(method, 0);
		CxList source = All.NewCxList();
		CxList size = All.NewCxList();
		
		if(srcSndParamSizeThrParam.Contains(methodName))
		{
			source = parameters.GetParameters(method, 1);
			size = parameters.GetParameters(method, 2);	
		}
		else if(srcSndParamSizeFthParam.Contains(methodName))
		{
			source = parameters.GetParameters(method, 1);
			size = parameters.GetParameters(method, 3);
		}
		else if(srcThrParamSizeSndParam.Contains(methodName))
		{
			source = parameters.GetParameters(method, 2);
			size = parameters.GetParameters(method, 1);
		}
		else if(srcFthParamSizeSndParam.Contains(methodName))
		{
			size = parameters.GetParameters(method, 1);
			source = parameters.GetParameters(method);
			
			CxList paramsToRemove = parameters.GetParameters(method, 0);
			paramsToRemove.Add(size); //second parameter
			paramsToRemove.Add(parameters.GetParameters(method, 2));
			source -= paramsToRemove;
		}
		
		// Keep useful nodes
		CxList sizeWithSize = sizeWithValue.GetByAncs(size);
		CxList destinationWithSize = nodesWithAbsVal.GetByAncs(destination);
		
		// Check if the size parameter contains size/strlen methods, which act as sanitizers
		CxList sizeInfluencedBySanitizer = sizeWithSize.InfluencedBy(sizeParameters);
		
		// The size limit is created with a strlen/sizeof function
		bool sanitized = sizeInfluencedBySanitizer.Count > 0;
		if (!sanitized)
		{
			// Sanitized by allocation based on size limit
			CxList memAllocSanitized = memAllocParams.FindAllReferences(sizeWithSize)
				.DataInfluencingOn(memAllocs).DataInfluencingOn(destinationWithSize);
			sanitized = memAllocSanitized.Count > 0;
		}
		
		if (sanitized)
		{
			continue;
		}
		
		// Compare destination buffer of size limit
		bool equal = CompareExprsByAbsVal(destinationWithSize, "==", sizeWithSize);
		if (equal)
		{
			continue;
		}

		bool smaller = CompareExprsByAbsVal(destinationWithSize, "<", sizeWithSize);
		bool greater = CompareExprsByAbsVal(destinationWithSize, ">", sizeWithSize);
		
		if (!smaller && greater)
		{
			continue;
		}
		CxList inputToSize = sizeWithSize.InfluencedBy(inputs);
		if (inputToSize.Count > 0) //Size parameter is influenced by input
		{
			CxList vulnerability = inputToSize.ConcatenateAllPaths(destinationWithSize, false);
			result.Add(vulnerability);
		}
		else if (smaller)
		{
			CxList sourceWithSize = nodesWithAbsVal.GetByAncs(source);
			CxList vulnerability = sourceWithSize.ConcatenateAllPaths(destinationWithSize, false);
			result.Add(vulnerability);
		}
	}
}