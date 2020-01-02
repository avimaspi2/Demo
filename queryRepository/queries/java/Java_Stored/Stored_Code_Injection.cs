CxList inputs = Find_Read();
inputs.Add(Find_DB_Out());

CxList sanitize = Find_General_Sanitize();
sanitize.Add(Find_Integers());

CxList code = Find_Code_Injection_Outputs();

result = inputs.InfluencingOnAndNotSanitized(code, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm)
	.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);