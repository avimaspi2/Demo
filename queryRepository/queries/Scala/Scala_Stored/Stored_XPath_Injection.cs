CxList XPath = Find_XPath_Outputs();
CxList sanitized = Find_XPath_Sanitize();
CxList inputs = Find_Read() + Find_DB_Out();

result = XPath.InfluencedByAndNotSanitized(inputs, sanitized).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);