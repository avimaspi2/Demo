CxList db = Find_DB_Out();
CxList read = Find_Read_NonDB();
CxList inputs = db + read;
CxList outputs = Find_XSS_Outputs();
CxList sanitize = Find_XSS_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

/*
Cleanup of long paths, no longer needed:
CxList potentialInputs = All * inputs.InfluencingOnAndNotSanitized(outputs, sanitize);

sanitize.Add(inputs);
foreach (CxList curInput in potentialInputs)
{
	result.Add(curInput.InfluencingOnAndNotSanitized(outputs, sanitize - curInput));
}
*/