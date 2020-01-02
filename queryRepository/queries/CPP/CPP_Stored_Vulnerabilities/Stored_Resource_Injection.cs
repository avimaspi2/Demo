CxList inputs = Find_DB_Out();
CxList resources = Find_Resource_Injection_Outputs();
CxList sanitizers = Find_Resource_Injection_Sanitizers();

result = inputs.InfluencingOnAndNotSanitized(resources, sanitizers);