CxList inputs = Find_Read();
inputs.Add(Find_DB_Out());

CxList sanitized = Find_XPath_Sanitize();

CxList XPath = Find_XPath_Outputs();

result = XPath.InfluencedByAndNotSanitized(inputs, sanitized).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);