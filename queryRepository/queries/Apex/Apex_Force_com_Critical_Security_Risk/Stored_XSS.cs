CxList inputs = Find_DB_Input();
CxList outputs = Find_Unsafe_Outputs();
CxList sanitized = Find_XSS_Sanitize() + Find_Test_Code();

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);