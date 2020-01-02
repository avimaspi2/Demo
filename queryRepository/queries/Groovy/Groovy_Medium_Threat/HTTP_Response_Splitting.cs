CxList header_inputs = Find_Headers();

CxList sanitize = Find_Splitting_Sanitizer();

CxList inputs = Find_Interactive_Inputs() - header_inputs;

CxList outputs = Find_Header_Outputs();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);