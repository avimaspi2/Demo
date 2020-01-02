CxList inputs = Find_Inputs();
CxList format = Get_Format_Parameter();
CxList sanitize = Find_Integers();

// Influence of input on format
result = inputs.InfluencingOnAndNotSanitized(format, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);

// Format that is also an input
// (We need a loop, because the input might be a flow)
foreach (CxList inFormat in inputs * format - sanitize)
{
	result.Add(inFormat.ConcatenateAllPaths(All.FindByParameters(inFormat)));
}

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);