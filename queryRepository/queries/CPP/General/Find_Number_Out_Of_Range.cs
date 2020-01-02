/*
	Find_Number_Out_Of_Range
	min 			- minimum value of the range
	max 			- maximum value of the range
	numberType	    - (OPTIONAL) the type of the number (i.e. "shor", "int", "long")
    signedness	    - (OPTIONAL) the Signedness of the number (signed or unsigned). 
*/ 
if (param.Length >= 2 && param.Length <= 4)
{	
	long? min = (long?) param[0];
	long? max = (long?) param[1];	
	CxList variables = All;
	TypeSizeModifiers numberType = TypeSizeModifiers.Default;
	TypeSignednessModifiers signedness = TypeSignednessModifiers.Unknown;
	bool useTypeSizeModifiers = false;
	bool useTypeSignednessModifiers = false;
	CxList builtinTypes = Find_Builtin_Types();
	CxList integers = builtinTypes.FindByType("int");
	CxList chars = builtinTypes.FindByType("char");
	
	if ( param.Length >= 3 ) 
	{
		string type = param[2] as string;
		
		switch (type)
		{
			case "short":
				numberType = TypeSizeModifiers.Short;
				useTypeSizeModifiers = true;
				variables = integers;
				break;
			case "long":
				numberType = TypeSizeModifiers.Long;
				useTypeSizeModifiers = true;
				variables = integers;
				break;
			case "longlong":
				numberType = TypeSizeModifiers.LongLong;
				useTypeSizeModifiers = true;
				variables = integers;
				break;
			case "int":
				variables = integers;
				numberType = TypeSizeModifiers.Default;
				useTypeSizeModifiers = true;
				break;
			case "char":
				variables = chars;
				numberType = TypeSizeModifiers.Default;
				useTypeSizeModifiers = true;
				break;
		}
	}
		
	if ( param.Length == 4 ) 
	{
		signedness = (TypeSignednessModifiers) param[3];
		useTypeSignednessModifiers = true;
	}
		
	if (useTypeSizeModifiers)
	{
		if (useTypeSignednessModifiers)
		{	
			variables = variables.FindByTypeModifiers(signedness, numberType);
		}
		else
		{
			variables = variables.FindByTypeModifiers(numberType);
		}
	}

	CxList absNumbers = variables.FindByAbstractValue(abstractValue => abstractValue is IntegerIntervalAbstractValue);
	IAbstractValue minNotAcceptedRange = new IntegerIntervalAbstractValue(Int64.MinValue, min);
	IAbstractValue maxNotAcceptedRange = new IntegerIntervalAbstractValue(max, Int64.MaxValue);	 
	
	result.Add(absNumbers.FindByAbstractValue(abstractValue => 
		abstractValue.IncludedIn(minNotAcceptedRange) ||
		abstractValue.IncludedIn(maxNotAcceptedRange)));	

}
else
{
	cxLog.WriteDebugMessage("This general query requires 3 or 4 arguments.");	
}