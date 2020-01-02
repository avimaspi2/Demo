CxList inputs = Find_Interactive_Inputs(); 
CxList exec = Find_Command_Injection_Outputs();
CxList sanitize = Find_Command_Injection_Sanitize();

result = exec.InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm)
	+ exec * inputs;

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);