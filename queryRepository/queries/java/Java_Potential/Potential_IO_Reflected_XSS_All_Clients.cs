CxList inputs = Find_Potential_Inputs();
CxList outputs = Find_Potential_Outputs();

CxList sanitized = Find_XSS_Sanitize(); 
sanitized.Add(Find_Interactive_Inputs()); 
sanitized.Add(Find_Interactive_Outputs()); 
sanitized.Add(Find_DB_In());

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized);