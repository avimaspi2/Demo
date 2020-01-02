/*
	This query finds flows from user input to their use as a file path
*/
if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList inputs = Kony_UI_Inputs();
	CxList outputs = Kony_FileSystem_Outputs();
	CxList sanitizers = Sanitize();
	
	result = outputs.InfluencedByAndNotSanitized(inputs, sanitizers);
}