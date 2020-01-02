CxList inputs = Find_Potential_Inputs();
CxList eval = Find_Outputs_CodeInjection();
CxList sanitize = Sanitize();

CxList lambda = Find_LambdaExpr();
CxList returns = Find_ReturnStmt();

sanitize.Add(All.FindByFathers(returns).GetByAncs(lambda));

result.Add(eval.FindByType(typeof(Param)) * inputs - sanitize);
result.Add(eval.InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm));
result.Add(Find_Source_Equals_Sink(inputs, eval));