/*
	This query finds flows from user inputs to their use as an URL in HTTP request
*/
if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList inputs = Kony_UI_Inputs();
	CxList outputs = Kony_HTTP_Outputs();
	CxList sanitizers = Sanitize();
	
	result = outputs.InfluencedByAndNotSanitized(inputs, sanitizers);
}