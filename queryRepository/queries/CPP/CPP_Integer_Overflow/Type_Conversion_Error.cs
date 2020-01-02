/* ***********************************************
	Find Chars and Integers by type and signedness and all
*/
CxList builtinTypes = Find_Builtin_Types();
CxList numbers = builtinTypes.FindByType("int");
CxList chars = builtinTypes.FindByType("char");
CxList biggerDataTypes;

// Finding signed char
CxList signedChar = All.NewCxList();
signedChar.Add(chars.FindByTypeModifiers(TypeSignednessModifiers.Signed));
signedChar.Add(chars.FindByTypeModifiers(TypeSignednessModifiers.Unknown));
// Finding unsigned char
CxList unsignedChar = chars.FindByTypeModifiers(TypeSignednessModifiers.Unsigned);

// Finding signed short
CxList signedShort = All.NewCxList();
signedShort.Add(numbers.FindByTypeModifiers(TypeSignednessModifiers.Signed, TypeSizeModifiers.Short));
signedShort.Add(numbers.FindByTypeModifiers(TypeSignednessModifiers.Unknown, TypeSizeModifiers.Short));
// Finding unsigned short
CxList unsignedShort = numbers.FindByTypeModifiers(TypeSignednessModifiers.Unsigned, TypeSizeModifiers.Short);

// Finding signed Int
CxList signedInt = All.NewCxList();
signedInt.Add(numbers.FindByTypeModifiers(TypeSignednessModifiers.Signed, TypeSizeModifiers.Default));
signedInt.Add(numbers.FindByTypeModifiers(TypeSignednessModifiers.Unknown, TypeSizeModifiers.Default));
// Finding unsigned Int
CxList unsignedInt = numbers.FindByTypeModifiers(TypeSignednessModifiers.Unsigned, TypeSizeModifiers.Default);

// Finding signed Long Int
CxList signedLong = All.NewCxList();
signedLong.Add(numbers.FindByTypeModifiers(TypeSignednessModifiers.Signed, TypeSizeModifiers.Long));
signedLong.Add(numbers.FindByTypeModifiers(TypeSignednessModifiers.Unknown, TypeSizeModifiers.Long));
// Finding unsigned Long Int 
CxList unsignedLong = numbers.FindByTypeModifiers(TypeSignednessModifiers.Unsigned, TypeSizeModifiers.Long);

// Finding signed Long Long Int
CxList signedLongLong = All.NewCxList();
signedLongLong.Add(numbers.FindByTypeModifiers(TypeSignednessModifiers.Signed, TypeSizeModifiers.LongLong));
signedLongLong.Add(numbers.FindByTypeModifiers(TypeSignednessModifiers.Unknown, TypeSizeModifiers.LongLong));
// Finding unsigned Long Long Int
CxList unsignedLongLong = numbers.FindByTypeModifiers(TypeSignednessModifiers.Unsigned, TypeSizeModifiers.LongLong);

/* ***********************************************
	Finding Chars and Integers casted to a smalller data type
*/
// Possible overflows to signed char
biggerDataTypes = All.NewCxList();
biggerDataTypes.Add(
	unsignedChar,
	signedShort,
	unsignedShort,
	signedInt,
	unsignedInt,
	signedLong,
	unsignedLong,
	signedLongLong,
	unsignedLongLong);

result.Add(biggerDataTypes.GetAssignee() * signedChar);

// Possible overflows to unsigned short
biggerDataTypes = All.NewCxList();
biggerDataTypes.Add(unsignedShort, unsignedInt, unsignedLong, unsignedLongLong);

result.Add(biggerDataTypes.GetAssignee() * unsignedChar);

// Possible overflows to signed short
biggerDataTypes = All.NewCxList();
biggerDataTypes.Add(
	unsignedShort,
	signedInt,
	unsignedInt,
	signedLong,
	unsignedLong,
	signedLongLong,
	unsignedLongLong);

result.Add(biggerDataTypes.GetAssignee() * signedShort);

// Possible overflows to unsigned short
biggerDataTypes = All.NewCxList();
biggerDataTypes.Add(unsignedInt, unsignedLong, unsignedLongLong);

result.Add(biggerDataTypes.GetAssignee() * unsignedShort);

// Possible overflows to signed int
biggerDataTypes = All.NewCxList();
biggerDataTypes.Add(
	unsignedInt,  // Not applicable to x64, so comment this in that case.
	unsignedLong,
	signedLongLong,
	unsignedLongLong);

result.Add(biggerDataTypes.GetAssignee() * signedInt);

// Possible overflows to unsigned int
biggerDataTypes = unsignedLongLong;
result.Add(biggerDataTypes.GetAssignee() * unsignedInt);

// Possible overflows to signed Long int
biggerDataTypes = All.NewCxList();
biggerDataTypes.Add(unsignedInt, unsignedLong, signedLongLong, unsignedLongLong);

result.Add(biggerDataTypes.GetAssignee() * signedLong);

// Possible overflows to unsigned long
biggerDataTypes = unsignedLongLong;
result.Add(biggerDataTypes.GetAssignee() * unsignedLong);

// Possible overflows to signed long
biggerDataTypes = unsignedLongLong;
result.Add(biggerDataTypes.GetAssignee() * signedLongLong);


/* ***********************************************
	Find Variables assigned to unsigned types
*/
// As a sanitizer, consider all predictable positive values:
CxList absNumbers = numbers.FindByAbstractValue(abstractValue => abstractValue is IntegerIntervalAbstractValue);
IAbstractValue positiveValuesRange = new IntegerIntervalAbstractValue(0, null);

CxList positiveValues = absNumbers.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(positiveValuesRange));
	
// Possible overflows to unsigned short

biggerDataTypes = All.NewCxList();
biggerDataTypes.Add(signedChar, signedShort, signedInt, signedLong, signedLongLong);

CxList smallDataTypes = All.NewCxList();
smallDataTypes.Add(unsignedShort, unsignedInt, unsignedLong, unsignedLongLong);

// Remove the predictable positive values, which is possible to cast:
result.Add((biggerDataTypes.GetAssignee() * smallDataTypes) - positiveValues);


/* ***********************************************
	Find floating points being passed to allocation methods
*/
//Finding all floating point variable and methods
CxList allMethods = Find_Methods();
CxList badAllocationValues = All.NewCxList();
badAllocationValues.Add(builtinTypes.FindByType("float"));
badAllocationValues.Add(builtinTypes.FindByType("double"));
badAllocationValues.Add(allMethods.FindAllReferences(All.FindByReturnType("float")));
badAllocationValues.Add(allMethods.FindAllReferences(All.FindByReturnType("double")));

IAbstractValue negativeValues = new IntegerIntervalAbstractValue(null, -1);
badAllocationValues.Add(absNumbers.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(negativeValues)));

//Finding all elements being used inside Allocation methods
CxList allocationMethods = Find_Memory_Allocation();
allocationMethods.Add(Find_Memory_Allocation_Dynamic());
allocationMethods -= Find_ArrayCreateExpr();
allocationMethods -= Find_ObjectCreations();
CxList allocationMethodParameters = All.GetParameters(allocationMethods);
CxList everythingUnderAllocationMethodsParams = All.GetByAncs(allocationMethodParameters);

//Find sanitizers of allocation methods (ie variables being used on other methods)
CxList paramMethods = allMethods * allocationMethodParameters;
CxList allocationSanitizers = All.GetParameters(paramMethods);

//All allocation sinks (nodes that are floating points as paramater of allocation method)
CxList badValuesPassedToAllocationMethods = (everythingUnderAllocationMethodsParams - allocationSanitizers) * badAllocationValues;

//Add floating points passed to allocation methods to final result:
result.Add(badValuesPassedToAllocationMethods);