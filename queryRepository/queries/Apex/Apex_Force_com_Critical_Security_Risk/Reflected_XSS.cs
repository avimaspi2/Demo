CxList inputs = Find_Interactive_Inputs();
CxList outputs = Find_Unsafe_Outputs();
CxList sanitized = Find_XSS_Sanitize() + Find_Test_Code() + Find_DB();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitized);

result -= Find_Test_Code();