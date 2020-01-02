if (CGI().Count > 0){
	
	CxList inputs = Find_Interactive_Inputs();
	CxList outputs = Find_Console_Outputs();

	CxList sanitized = Find_XSS_Sanitize();
	sanitized.Add(Find_DB_In());
	sanitized.Add(Find_Files_Open());

	result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized);
}