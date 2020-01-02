CxList sanitize = NodeJS_Find_Splitting_Sanitizer();

CxList inputs = NodeJS_Find_Interactive_Inputs();
CxList outputs = NodeJS_Find_Header_Outputs();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);