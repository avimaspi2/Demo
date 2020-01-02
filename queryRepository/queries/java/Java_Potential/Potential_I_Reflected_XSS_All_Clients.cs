CxList inputs = Find_Potential_Inputs();
CxList outputs = Find_XSS_Outputs();

CxList sanitized = Find_XSS_Sanitize();
sanitized.Add(Find_DB_In());

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized);