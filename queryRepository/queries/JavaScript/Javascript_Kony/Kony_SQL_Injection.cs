/*
	This query finds flows from user input to sql query
*/
if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList inputs = Kony_UI_Inputs();
	CxList outputs = Kony_DB_In();
	CxList sanitizers = Sanitize();
	
	result = outputs.InfluencedByAndNotSanitized(inputs, sanitizers);
}