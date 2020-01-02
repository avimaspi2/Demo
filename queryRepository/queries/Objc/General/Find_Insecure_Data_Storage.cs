// param[0] is the sensitive data, param[1] (if present) is CxList of outputs which are considered safe/secure
CxList outputs = Find_Insecure_Write();
outputs.Add(Find_DB_In_UserDefaults());

CxList sensitive;
if (param.Length >0)
{
	sensitive = param[0] as CxList;
	if (param.Length == 2)
	{
		CxList safeOutput = param[1] as CxList;
		outputs -= safeOutput;
	}
}
else
{
	sensitive = Find_Personal_Info();
}

CxList sanitize = Find_General_Sanitize();

result = outputs.InfluencedByAndNotSanitized(sensitive, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
result.Add(outputs * sensitive);

// solve reduce flow issue!!!!!
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);