/*
	This query finds flows from stored data to sql query
*/
if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList inputs = Kony_DataStore_Inputs();
	inputs.Add(Kony_LocalStore_Inputs());
	inputs.Add(Kony_DB_Out());
	inputs.Add(Kony_FileSystem_Inputs());	
	
	CxList outputs = Kony_DB_In();
	CxList sanitizers = Sanitize();
	
	result = outputs.InfluencedByAndNotSanitized(inputs, sanitizers);
}