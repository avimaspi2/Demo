CxList inputs = Find_Read() + Find_DB_Out();
CxList code = Find_Code_Injection_Outputs();
CxList sanitize = Find_General_Sanitize() + Find_Integers();

result = inputs.InfluencingOnAndNotSanitized(code, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm)
	.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);