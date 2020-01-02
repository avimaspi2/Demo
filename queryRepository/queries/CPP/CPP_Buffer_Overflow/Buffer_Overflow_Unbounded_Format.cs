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

// Sanitizer 1
CxList strlenParams = All.GetParameters(Find_All_Strlen());
strlenParams.Add(unknownRefs.GetByAncs(strlenParams));

// Sanitizer 2
CxList memAllocParams = All.GetParameters(memAllocs);
memAllocParams.Add(unknownRefs.GetByAncs(memAllocParams));

// Methods that use format specifiers and could lead to Buffer Overflow
CxList methodsWithFormat = Find_BufferOverflow_ScanPrint_Funcs();

foreach(CxList fmtMethod in methodsWithFormat)
{
	CxList fmtMethodParams = All.GetParameters(fmtMethod);
	CxList fmtParam = fmtMethodParams.FindByAbstractValue(val => {
		if (val is StringAbstractValue) { 
			Match match = Regex.Match((val as StringAbstractValue).Content, @"%(\d)*(\*\.)?s");
			return match.Success;
		}
		return false;
	}); // StringLiterals or UnknownReferences

	CxList fmtStringLiteralParam = fmtParam.FindByType(typeof(StringLiteral));

	//If the parameter is not a StringLiteral, check if it's a reference of one (and get it)
	if (fmtStringLiteralParam.Count == 0)
	{
		CxList fmtUnknownRefParam = fmtParam.FindByType(typeof(UnknownReference));
		CxList stringRefs = stringLiterals.FindByAbstractValues(fmtUnknownRefParam);
		CxList correctStringRef = stringRefs.InfluencingOn(fmtUnknownRefParam)
			.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
		fmtStringLiteralParam = correctStringRef.FindByType(typeof(StringLiteral));
	}

	MethodInvokeExpr methodGraph = fmtMethod.TryGetCSharpGraph<MethodInvokeExpr>();
	StringLiteral stringGraph = fmtStringLiteralParam.TryGetCSharpGraph<StringLiteral>();
	if (methodGraph != null && stringGraph != null)
	{
		// Identify all format specifiers in the String (e.g. %s %d %i %10s %*.s)
		int matchNo = 0;
		MatchCollection matchCol = Regex.Matches(stringGraph.ShortName, @"%(\d*)(\'|\*|\.)*(\d*)(\w)");
		foreach (Match match in matchCol)
		{
			if (match.Groups[4].Value != "s")
			{
				matchNo++;
				continue; //If the specifier is not String, skip a parameter and go to the next one
			}
			
			if (match.Groups[2].Value == "*.")
			{
				matchNo++; // For the %*.s we are currently just skipping one parameter
			}
			
			CxList sourceParam = All.NewCxList();
			CxList destinationParam = All.NewCxList();
			CxList inputToSourceParam = All.NewCxList();

			// Identify the source and destination for this method
			// For methods that don't read directly from input, we also check if there's a flow from input to the source
			string methodName = methodGraph.ShortName;
			switch(methodName)
			{
				case "scanf":
					sourceParam = fmtMethod;
					destinationParam = All.GetByAncs(All.GetParameters(fmtMethod, matchNo + 1));
					break;
				case "vscanf":
					sourceParam = fmtMethod;
					destinationParam = All.GetByAncs(All.GetParameters(fmtMethod, 1));
					break;
				case "fscanf":
					sourceParam = All.GetByAncs(All.GetParameters(fmtMethod, 0));
					destinationParam = All.GetByAncs(All.GetParameters(fmtMethod, matchNo + 2));
					break;
				case "vfscanf":
					sourceParam = All.GetByAncs(All.GetParameters(fmtMethod, 0));
					destinationParam = All.GetByAncs(All.GetParameters(fmtMethod, 2));
					break;
				case "sscanf":
					sourceParam = All.GetByAncs(All.GetParameters(fmtMethod, 0));
					inputToSourceParam = sourceParam.InfluencedBy(inputs);
					destinationParam = All.GetByAncs(All.GetParameters(fmtMethod, matchNo + 2));
					break;
				case "vsscanf":
					sourceParam = All.GetByAncs(All.GetParameters(fmtMethod, 0));
					inputToSourceParam = sourceParam.InfluencedBy(inputs);
					destinationParam = All.GetByAncs(All.GetParameters(fmtMethod, 2));
					break;
				case "sprintf":
					sourceParam = All.GetByAncs(All.GetParameters(fmtMethod, matchNo + 2));
					inputToSourceParam = sourceParam.InfluencedBy(inputs);
					destinationParam = All.GetByAncs(All.GetParameters(fmtMethod, 0));
					break;
			}
			matchNo++;
            
			CxList sourceActualParams = sourceParam.FindByType(typeof(Param));
			if (sourceActualParams.Count > 0)
			{
				sourceParam = All.FindByFathers(sourceActualParams);
			}
			destinationParam = destinationParam.FindByType(typeof(UnknownReference));
			
			CxList sanitized = All.NewCxList();
			
			// Sanitized by length-check on input
			CxList strlenSanitized = sourceParam.DataInfluencedBy(strlenParams);
			sanitized.Add(strlenSanitized);
	
			// Sanitized by allocation based on input
			CxList memAllocSanitized = destinationParam.DataInfluencedBy(memAllocs)
				.DataInfluencedBy(All.FindAllReferences(sourceParam) * memAllocParams);
			sanitized.Add(memAllocSanitized);
			
			// Sanitized by usage of std::string
			CxList stdStringSanitized = destinationParam.FindByTypes(new string[] {"string", "std.string"});
			sanitized.Add(stdStringSanitized);
			
			if (sanitized.Count == 0)
			{	
				// If format spec contains number, check if it's less or equal than the destination buffer size
				int numberSpec;
				bool hasNumberSpecifier = int.TryParse(match.Groups[1].Value, out numberSpec);
				CxList lessThanSpecifier = All.NewCxList();
				if (hasNumberSpecifier)
				{
					lessThanSpecifier = destinationParam.FindByAbstractValue(val => {
						if (val is ObjectAbstractValue) {
							IntegerIntervalAbstractValue absValSpec = new IntegerIntervalAbstractValue(numberSpec);
							IntegerIntervalAbstractValue allocatedsize = (val as ObjectAbstractValue).AllocatedSize;
							return allocatedsize == null || allocatedsize.LessThanOrEqual(absValSpec) is TrueAbstractValue;
						}
						return false;
					});
				}
				
				// If the format specifier is unbounded (%s) or 
				// the destination buffer is smaller than the number specifier or 
				// we couldn't determine the size of the destination buffer
				if (match.Captures[0].Value == "%s" || lessThanSpecifier.Count > 0)
				{
					// There is flow from input to the source param (sscanf and sprintf only)
					if (inputToSourceParam.Count > 0)
					{
						CxList fromInputBufferOverflow = inputToSourceParam.ConcatenatePath(destinationParam, false);
						result.Add(fromInputBufferOverflow);
					}
					else
					{
						// Keep only nodes that can have size
						CxList destWithSize = destinationParam * unknownRefs;
						destWithSize.Add(destinationParam * stringLiterals);
						CxList srcWithSize = sourceParam * unknownRefs;
						srcWithSize.Add(sourceParam * stringLiterals);
						
						bool srcSmallerThanDest = CompareExprsByAbsValLT(srcWithSize, destWithSize);
						// Source parameter is not guaranteed to be smaller than destination parameter
						if (!srcSmallerThanDest)
						{
							srcWithSize.Add(sourceParam * methodInvokes); //To show better flow in scanf
							CxList boWithDestination = srcWithSize.ConcatenatePath(destWithSize, false);
							result.Add(boWithDestination);
						}
						else
						{
							bool destSmallerThanSrc = CompareExprsByAbsValLT(destWithSize, srcWithSize);
							// Destination parameter is smaller than source parameter
							if (destSmallerThanSrc) 
							{
								CxList sourceToDestination = srcWithSize.ConcatenatePath(destWithSize, false);
								result.Add(sourceToDestination);
							}
						}
					}
				}
			}
		}
	}
}