CxList sanitize = Find_Splitting_Sanitizer();
CxList inputs = Find_Interactive_Inputs() - Find_Headers();
CxList outputs = Find_Header_Outputs();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);