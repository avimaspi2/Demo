CxList inputs = Find_Interactive_Inputs();
CxList outputs = Find_Interactive_Outputs();
CxList sanitized = Find_XSS_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);