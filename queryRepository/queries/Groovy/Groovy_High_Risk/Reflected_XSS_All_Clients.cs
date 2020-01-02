CxList inputs = Find_Interactive_Inputs();
CxList outputs = Find_XSS_Outputs();

CxList sanitized = Find_XSS_Sanitize() + Find_DB_In();

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);