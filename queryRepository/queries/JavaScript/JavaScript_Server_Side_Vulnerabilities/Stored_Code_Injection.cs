CxList inputs = NodeJS_Find_Read();
inputs.Add(NodeJS_Find_DB_Out());

CxList output = NodeJS_Find_Outputs_CodeInjection();
CxList sanitize = NodeJS_Find_General_Sanitize();

result = output.InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
result.Add(inputs * output - sanitize);