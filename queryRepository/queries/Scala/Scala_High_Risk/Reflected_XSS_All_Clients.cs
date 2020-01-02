CxList inputs = Find_Interactive_Inputs();
CxList outputs = Find_XSS_Outputs();

CxList sanitized = Find_XSS_Sanitize(outputs) + Find_DB_In() + Find_Files_Open();

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized);