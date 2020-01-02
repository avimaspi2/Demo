CxList inputs = Find_Interactive_Inputs();
CxList outputs = Find_XSS_Outputs();

CxList sanitized = Find_XSS_Sanitize();
sanitized.Add(Find_DB_In());
sanitized.Add(Find_Files_Open());
sanitized -= Find_Decode_Encode(outputs);

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized, CxList.InfluenceAlgorithmCalculation.NewAlgorithm).ReduceFlowByPragma();
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);