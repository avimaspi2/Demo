CxList header_outputs =	
	Find_Header_Outputs();

CxList sanitize = Find_XSS_Sanitize(); 

CxList inputs = Find_Interactive_Inputs();

result = header_outputs.InfluencedByAndNotSanitized(inputs, sanitize);