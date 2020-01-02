CxList inputs = Find_Inputs_NoWindowLocation();
CxList Eval = Find_Outputs_CodeInjection();
CxList sanitize = Code_Injection_Sanitize();

result.Add(Eval.FindByType(typeof(Param)) * inputs - sanitize);

// Remove inputs which are implicitely passed as parameters to anonymous functions.
CxList imAParam = inputs.GetParameters(Find_ObjectCreations());
CxList lambda = Find_LambdaExpr();
imAParam.Add(inputs.GetParameters(lambda));
inputs -= imAParam;

result.Add((Eval).InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm));
result.Add(Find_Source_Equals_Sink(inputs, Eval));