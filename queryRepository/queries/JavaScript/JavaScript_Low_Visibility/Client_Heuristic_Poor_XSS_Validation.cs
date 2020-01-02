/*	Client_Heuristic_Poor_XSS_Validation

	This query looks for potential XSS attacks being
	sanitized by "weak" sanitizers. The idea is that
	the programmer might be thinking she is sanitizing
	against a XSS attack when she actually isn't.
*/

CxList inputs = Find_Inputs();
inputs.Add(Find_Storage_Inputs());

CxList outputs = Find_Outputs_XSS();
outputs.Add(Find_Vulnerable_SetAttribute());
outputs = outputs.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

CxList methods = Find_Methods();
CxList weakSanitizers = methods.FindByShortNames(new List<string> {"escape*", "encodeURI*", "_encodeURI*"});

CxList relevantWeakSanitizers = weakSanitizers
	.DataInfluencedBy(inputs).DataInfluencingOn(outputs);

CxList temp = inputs.DataInfluencingOn(weakSanitizers)
	.DataInfluencingOn(outputs, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);

foreach(CxList t in temp.GetCxListByPath())
{
	CxList sanitizer = t.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllNodes) * relevantWeakSanitizers;
	if(sanitizer.Count > 0)
	{
		// Remove weak sanitizers in loop statements
		CxList loops = sanitizer.GetAncOfType(typeof(IterationStmt));
		CxList relevantWeakSanitizersInLoop = sanitizer.GetByAncs(loops);
		if((sanitizer - relevantWeakSanitizersInLoop).Count > 0)
		{
			result.Add(t);
		}
	}
}

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);