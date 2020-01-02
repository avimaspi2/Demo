CxList inputs = Find_Potential_Inputs();
CxList outputs = Find_Outputs_XSS();
outputs = outputs - outputs.FindByAssignmentSide(CxList.AssignmentSide.Right);

CxList sanitize = Sanitize();
sanitize.Add(Find_XSS_Sanitize());

CxList outMethods = outputs.FindByType(typeof (MethodInvokeExpr));
CxList outParams = All.GetParameters(outMethods);
outParams -= outParams.FindByShortName("Lambda");//anonymus function will double results.
outputs -= outMethods;

CxList outputsParams = All.NewCxList();
outputsParams.Add(outParams);
outputsParams.Add(outputs);

CxList toParms = outputsParams.InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);

foreach (CxList curPath in toParms.GetCxListByPath())
{
	CxList lastNode = curPath.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	if ((lastNode * outParams).Count > 0)
	{
		CxList paramMethod = outMethods.FindByParameters(lastNode);
		if (paramMethod.Count == 1)
		{
			result.Add(curPath.ConcatenatePath(paramMethod, false));
		}
	}
	else 
	{
		result.Add(curPath);
	}
}

result.Add(outputs.FindByType(typeof(Param)) * inputs - sanitize);
result.Add(Find_Source_Equals_Sink(inputs, outputs) - sanitize);

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);