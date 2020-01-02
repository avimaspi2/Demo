CxList inputs = Find_Potential_Inputs();
CxList code = Find_Code_Injection_Outputs();
CxList sanitize = Find_General_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(code, sanitize);