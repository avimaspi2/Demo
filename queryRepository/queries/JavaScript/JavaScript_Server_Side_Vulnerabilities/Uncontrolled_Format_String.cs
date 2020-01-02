// Find all format strings that are affected by any type of input and not sanitized by length or size

// Find references of util.format calls
CxList util = Find_Require("util");

CxList formatMethInv = util.GetMembersOfTarget().FindByShortName("format");
// Get first parameter of method call (the format string)
CxList formatString = All.GetParameters(formatMethInv, 0);
// only interested in the values
formatString -= formatString.FindByType(typeof(Param));

CxList inputs = NodeJS_Find_Inputs();

CxList sanitize = NodeJS_Find_Integers();

result = inputs.InfluencingOnAndNotSanitized(formatString, sanitize,
	CxList.InfluenceAlgorithmCalculation.NewAlgorithm).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);