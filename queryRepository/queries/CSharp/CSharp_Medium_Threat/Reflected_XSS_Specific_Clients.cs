if(All.isWebApplication)
{
	CxList inputs = Find_Interactive_Inputs();
	CxList outputs = Find_Web_Outputs() - Find_XSS_Outputs() - Find_Safe_Response();

	CxList sanitized = Find_XSS_Sanitize();

	result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized);
}