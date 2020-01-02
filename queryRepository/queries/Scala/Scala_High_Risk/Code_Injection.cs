CxList inputs = Find_Interactive_Inputs();
CxList code = Find_Code_Injection_Outputs();
CxList sanitize = Find_General_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(code, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);