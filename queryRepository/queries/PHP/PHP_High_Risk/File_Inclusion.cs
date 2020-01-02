CxList inputs = Find_Interactive_Inputs();

CxList include = Find_File_Includes();

// find the file input sanitizers (number and string functions)
CxList sanitized = Find_File_Sanitizers();
sanitized.Add(Find_WP_File_Inclusion_Sanitize());

result = include.InfluencedByAndNotSanitized(inputs, sanitized).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);