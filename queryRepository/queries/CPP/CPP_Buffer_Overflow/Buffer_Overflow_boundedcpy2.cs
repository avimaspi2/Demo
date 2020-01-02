CxList boundedMethods = Find_Bounded_Methods();
CxList smallTypeList;
CxList bigTypeList;
CxList param0;
CxList param1;
CxList dest;
CxList src;
CxList builtIn = Find_Builtin_Types();
CxList doubles = builtIn.FindByType("double");
CxList floats = builtIn.FindByType("float");
CxList longs = builtIn.FindByType("long");
CxList ints = builtIn.FindByType("int");	
CxList chars = builtIn.FindByType("char");	
CxList shorts = builtIn.FindByType("short");	
CxList bools = builtIn.FindByType("bool");
CxList firstParam = All.GetParameters(boundedMethods, 0);
CxList secondParam = All.GetParameters(boundedMethods, 1);

foreach (CxList boundedMethod in boundedMethods)
{
	param0 = firstParam.GetParameters(boundedMethod, 0);
	param1 = secondParam.GetParameters(boundedMethod, 1);
	
	// Float
	bigTypeList = All.NewCxList(); bigTypeList.Add(doubles);
	smallTypeList = floats;

	dest = param0*smallTypeList;
	src = param1*bigTypeList;
	if ((dest.Count>0) && (src.Count>0))
	{
		result.Add(boundedMethod);
	}

	// Long
	bigTypeList.Add(smallTypeList);
	smallTypeList = longs;

	dest = param0*smallTypeList;
	src = param1*bigTypeList;
	if ((dest.Count>0) && (src.Count>0))
	{
		result.Add(boundedMethod);
	}

	// Integer
	bigTypeList.Add(smallTypeList);
	smallTypeList = ints;

	dest = param0*smallTypeList;
	src = param1*bigTypeList;
	if ((dest.Count>0) && (src.Count>0))
	{
		result.Add(boundedMethod);
	}

	// Char + Short
	bigTypeList.Add(smallTypeList);
	smallTypeList = chars + shorts;
	
	dest = param0*smallTypeList;
	src = param1*bigTypeList;
	if ((dest.Count>0) && (src.Count>0))
	{
		result.Add(boundedMethod);
	}

	// Bool
	bigTypeList.Add(smallTypeList);
	smallTypeList = bools;

	dest = param0*smallTypeList;
	src = param1*bigTypeList;
	if ((dest.Count>0) && (src.Count>0))
	{
		result.Add(boundedMethod);
	}
}

// Also bcopy, which has the parameters swapped
// (other ideas?)
boundedMethods = Find_Methods().FindByShortName("bcopy");
firstParam = All.GetParameters(boundedMethods, 1);
secondParam = All.GetParameters(boundedMethods, 0);

foreach (CxList boundedMethod in boundedMethods)
{
	param0 = firstParam.GetParameters(boundedMethod, 1);
	param1 = secondParam.GetParameters(boundedMethod, 0);
	
	// Float
	bigTypeList = All.NewCxList(); bigTypeList.Add(doubles);
	smallTypeList = floats;

	dest = param0 * smallTypeList;
	src = param1 * bigTypeList;
	if ((dest.Count > 0) && (src.Count > 0))
	{
		result.Add(boundedMethod);
	}

	// Long
	bigTypeList.Add(smallTypeList);
	smallTypeList = longs;

	dest = param0 * smallTypeList;
	src = param1 * bigTypeList;
	if ((dest.Count > 0) && (src.Count > 0))
	{
		result.Add(boundedMethod);
	}

	// Integer
	bigTypeList.Add(smallTypeList);
	smallTypeList = ints;

	dest = param0 * smallTypeList;
	src = param1 * bigTypeList;
	if ((dest.Count > 0) && (src.Count > 0))
	{
		result.Add(boundedMethod);
	}

	// Char + Short
	bigTypeList.Add(smallTypeList);
	smallTypeList = chars + shorts;
	
	dest = param0 * smallTypeList;
	src = param1 * bigTypeList;
	if ((dest.Count > 0) && (src.Count > 0))
	{
		result.Add(boundedMethod);
	}

	// Bool
	bigTypeList.Add(smallTypeList);
	smallTypeList = bools;

	dest = param0 * smallTypeList;
	src = param1 * bigTypeList;
	if ((dest.Count > 0) && (src.Count > 0))
	{
		result.Add(boundedMethod);
	}
}