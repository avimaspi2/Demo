// Find all format strings that are affected by any type of input and not sanitized by length or size
CxList formatString = Find_Uncontrolled_Format_String();
formatString = All.GetParameters(formatString, 0);
formatString -= formatString.FindByType(typeof(Param));


CxList inputs = Find_Inputs();

// Find all sanitizations and leave only the size and length
CxList sanitize = Find_Sanitize();
sanitize = Find_Uncontrolled_Format_String_Sanitize(sanitize);

result = inputs.InfluencingOnAndNotSanitized(formatString, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);