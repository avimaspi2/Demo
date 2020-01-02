if(All.isWebApplication)
{
	CxList inputs = Find_Interactive_Inputs();
	CxList outputs = Find_Interactive_Outputs() - Find_XSS_Outputs();
	
	outputs -= All.FindByMemberAccess("Textbox.Text", false);
	
	CxList outpParam = All.GetParameters(outputs);
	CxList sanitized = Find_XSS_Sanitize();
	result = outpParam.InfluencedByAndNotSanitized(inputs, sanitized);
	result = outputs.DataInfluencedBy(result, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
	
	CxList outpMemAcs = outputs.FindByType(typeof(MemberAccess));
	CxList outputsRes = outpMemAcs.InfluencedByAndNotSanitized(inputs, sanitized);
	result.Add(outputsRes);
}