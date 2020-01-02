CxList storedOut = Find_Cookie();
storedOut.Add(Find_Storage_Outputs());
storedOut.Add(Find_DB_Out());
CxList Eval = Find_Outputs_CodeInjection();
CxList sanitize = Code_Injection_Sanitize();

result.Add(Eval.FindByType(typeof(Param)) * storedOut - sanitize);

result.Add(Eval.InfluencedByAndNotSanitized(storedOut, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm));
result.Add(Find_Source_Equals_Sink(storedOut, Eval));