/*
	Find overflows by type 
*/
CxList overFlowVariables = All.NewCxList();

//Constants:
long MIN_UNSIGNED_VALUE = -1;
long CHAR_MIN_SIGNED_VALUE = -129; // minimum signed value - 1
long CHAR_MAX_SIGNED_VALUE = 128; // maximum signed value + 1
long CHAR_MAX_UNSIGNED_VALUE = 256; // maximum unsigned value + 1
long SHORT_MIN_SIGNED_VALUE = -32769; // minimum signed value - 1
long SHORT_MAX_SIGNED_VALUE = 32768; // maximum signed value + 1
long SHORT_MAX_UNSIGNED_VALUE = 65536; // maximum unsigned value + 1
long INT_MIN_SIGNED_VALUE = -2147483649; // minimum signed value - 1
long INT_MAX_SIGNED_VALUE = 2147483648; // maximum signed value + 1
long INT_MAX_UNSIGNED_VALUE = 4294967296; // maximum unsigned value + 1
long LONG_MIN_SIGNED_VALUE = -2147483649; // minimum signed x86 value -1
long LONG_MAX_SIGNED_VALUE = 2147483648; // maximum signed x86  value +1
//long LONG_MIN_SIGNED_VALUE = -9223372036854775808; // minimum signed x64 value
//long LONG_MAX_SIGNED_VALUE = 9223372036854775807; // maximum signed x64  value
long LONGLONG_MIN_SIGNED_VALUE = -9223372036854775808; // minimum signed value
long LONGLONG_MAX_SIGNED_VALUE = 9223372036854775807; // maximum signed value


// Finding all UNSIGNED CHAR numbers with overflow/underflow values 
CxList unsignedCharOverflows = Find_Number_Out_Of_Range(MIN_UNSIGNED_VALUE, CHAR_MAX_UNSIGNED_VALUE, "char", TypeSignednessModifiers.Unsigned);
overFlowVariables.Add(unsignedCharOverflows);
// Finding all SIGNED CHAR numbers with overflow/underflow values 
CxList signedCharOverflows = Find_Number_Out_Of_Range(CHAR_MIN_SIGNED_VALUE, CHAR_MAX_SIGNED_VALUE, "char", TypeSignednessModifiers.Signed);
signedCharOverflows.Add(Find_Number_Out_Of_Range(CHAR_MIN_SIGNED_VALUE, CHAR_MAX_SIGNED_VALUE, "char", TypeSignednessModifiers.Unknown));
overFlowVariables.Add(signedCharOverflows);
// Finding all UNSIGNED SHORT numbers with overflow/underflow values 
CxList unsignedShortOverflows = Find_Number_Out_Of_Range(MIN_UNSIGNED_VALUE, SHORT_MAX_UNSIGNED_VALUE, "short", TypeSignednessModifiers.Unsigned);
overFlowVariables.Add(unsignedShortOverflows);
// Finding all SIGNED SHORT numbers with overflow/underflow values 
CxList signedShortOverflows = Find_Number_Out_Of_Range(SHORT_MIN_SIGNED_VALUE, SHORT_MAX_SIGNED_VALUE, "short",TypeSignednessModifiers.Signed);
signedShortOverflows.Add(Find_Number_Out_Of_Range(SHORT_MIN_SIGNED_VALUE, SHORT_MAX_SIGNED_VALUE, "short",TypeSignednessModifiers.Unknown));
overFlowVariables.Add(signedShortOverflows);

// Finding all UNSIGNED INT numbers with overflow/underflow values
CxList unsignedIntOverflows = Find_Number_Out_Of_Range(MIN_UNSIGNED_VALUE, INT_MAX_UNSIGNED_VALUE, "int", TypeSignednessModifiers.Unsigned);
overFlowVariables.Add(unsignedIntOverflows);
// Finding all SIGNED INT numbers with overflow/underflow values
CxList signedIntOverflows = Find_Number_Out_Of_Range(INT_MIN_SIGNED_VALUE, INT_MAX_SIGNED_VALUE, "int",TypeSignednessModifiers.Signed);
signedIntOverflows.Add(Find_Number_Out_Of_Range(INT_MIN_SIGNED_VALUE, INT_MAX_SIGNED_VALUE, "int",TypeSignednessModifiers.Unknown));
overFlowVariables.Add(signedIntOverflows);

// Finding all SIGNED LONG numbers with overflow/underflow values
CxList signedLongOverflows = Find_Number_Out_Of_Range(LONG_MIN_SIGNED_VALUE, LONG_MAX_SIGNED_VALUE, "long",TypeSignednessModifiers.Signed);
signedLongOverflows.Add(Find_Number_Out_Of_Range(LONG_MIN_SIGNED_VALUE, LONG_MAX_SIGNED_VALUE, "long",TypeSignednessModifiers.Unknown));
overFlowVariables.Add(signedLongOverflows);

// Finding all SIGNED LONGLONG numbers with overflow/underflow values
CxList signedLongLongOverflows = Find_Number_Out_Of_Range(LONGLONG_MIN_SIGNED_VALUE, LONGLONG_MAX_SIGNED_VALUE, "longlong",TypeSignednessModifiers.Signed);
signedLongLongOverflows.Add(Find_Number_Out_Of_Range(LONGLONG_MIN_SIGNED_VALUE, LONGLONG_MAX_SIGNED_VALUE, "longlong",TypeSignednessModifiers.Unknown));
overFlowVariables.Add(signedLongLongOverflows);


/*
	Removing duplicated results:
*/
CxList nodesToRemove = All.NewCxList();
CxList flowsBetweenVariables = overFlowVariables.InfluencingOn(overFlowVariables);
foreach(CxList flow in flowsBetweenVariables.GetCxListByPath()){
	nodesToRemove.Add(flow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));
}
overFlowVariables = overFlowVariables - nodesToRemove;

/*
	Create flows when possible (won't create flows on constant assignments)
*/
CxList binaryExpressions = Find_BinaryExpr();
CxList flowsToOverflows = binaryExpressions.InfluencingOn(overFlowVariables);

/*
	Flows to Overflows and variables with predictable overflows
*/
nodesToRemove = All.NewCxList();
foreach(CxList flow in flowsToOverflows.GetCxListByPath()){
	nodesToRemove.Add(flow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));
}
overFlowVariables -= nodesToRemove;

/*
	Return the results
*/
result.Add(overFlowVariables);
result.Add(flowsToOverflows.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow));