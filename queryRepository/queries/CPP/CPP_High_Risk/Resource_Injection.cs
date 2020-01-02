CxList inputs = Find_Interactive_Inputs();
CxList resources = Find_Resource_Injection_Outputs();
CxList sanitizers = Find_Resource_Injection_Sanitizers();

result = inputs.InfluencingOnAndNotSanitized(resources,sanitizers);