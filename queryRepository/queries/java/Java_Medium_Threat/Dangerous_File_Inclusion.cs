CxList inputs = Find_Interactive_Inputs();
CxList include = Find_File_Inclusion();
CxList sanitize = Find_General_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(include, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);