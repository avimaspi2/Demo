CxList inputs = Find_Visualforce_Remoting_Inputs();

CxList Eval = Find_Outputs_CodeInjection();
CxList sanitize = Sanitize();

CxList paramsInputs = Eval.FindByType(typeof(Param)) * inputs;
paramsInputs -= sanitize;

result.Add(paramsInputs);
result.Add((Eval).InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm));
result.Add(Find_Source_Equals_Sink(inputs, Eval));