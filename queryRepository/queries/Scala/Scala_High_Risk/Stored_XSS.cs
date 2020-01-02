CxList db = Find_DB_Out();
CxList read = Find_Read_NonDB() + Find_FileSystem_Read();
CxList inputs = db + read;
CxList outputs = Find_XSS_Outputs();
CxList sanitize = Find_XSS_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);