CxList inputs = Find_Interactive_Inputs() - Find_Interactive_Inputs_User();
CxList outputs = Find_XSS_Outputs();
CxList sanitized = Find_XSS_Sanitize();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitized);