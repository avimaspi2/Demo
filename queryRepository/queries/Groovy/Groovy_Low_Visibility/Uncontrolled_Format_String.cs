// Find all format strings that are affected by any type of input and not sanitized by length or size
CxList formatString = 
	All.FindByMemberAccess("out.format") + 
	All.FindByMemberAccess("out.printf");
formatString = All.GetParameters(formatString, 0);
formatString -= formatString.FindByType(typeof(Param));

CxList inputs = Find_Inputs();

// Find all sanitizations and leave only the size and length
CxList sanitize = Find_Sanitize();
sanitize = 
	sanitize.FindByShortName("size") +
	sanitize.FindByShortName("length");

result = inputs.InfluencingOnAndNotSanitized(formatString, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);