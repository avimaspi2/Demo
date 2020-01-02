CxList inputs = Find_Interactive_Inputs();
CxList outputs = Find_Interactive_Outputs();
CxList sanitized = Find_XSS_Sanitize() + Find_DB();

result = 
	outputs.InfluencedByAndNotSanitized(inputs, sanitized) + 
	inputs * outputs;