CxList inputs = Find_Interactive_Inputs();
CxList requests = Find_Remote_Requests();
CxList sanitizers = Find_Remote_Requests_Sanitize();
	
result = requests.InfluencedByAndNotSanitized(inputs, sanitizers);