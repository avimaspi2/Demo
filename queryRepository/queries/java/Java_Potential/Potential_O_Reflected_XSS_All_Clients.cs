CxList inputs = Find_Interactive_Inputs();
CxList outputs = Find_Potential_Outputs();

CxList sanitized = Find_XSS_Sanitize();
sanitized.Add(Find_DB_In());

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized);