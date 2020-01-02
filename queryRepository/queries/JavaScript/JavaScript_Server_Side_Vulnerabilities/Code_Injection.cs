CxList inputs = NodeJS_Find_Interactive_Inputs();
CxList output = NodeJS_Find_Outputs_CodeInjection();
CxList sanitize = NodeJS_Find_General_Sanitize();

result = output.InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm)
	+ (inputs * output - sanitize);