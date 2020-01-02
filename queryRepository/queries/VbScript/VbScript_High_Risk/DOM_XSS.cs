CxList outputs = Find_Outputs_XSS(); 
CxList inputs = Find_Inputs();
CxList sanitize = Find_Sanitize();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);