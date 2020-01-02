CxList outputs = Find_Outputs_XSS();
outputs.Add(Find_Outputs_Redirection().FindByShortNames(new List<string>{ "location", "href" }));
CxList inputs = Find_Inputs_NoWindowLocation();
CxList sanitize = Sanitize();
sanitize.Add(Find_XSS_Sanitize());
sanitize.Add(Find_SAPUI_XSS_Sanitized_Outputs());

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
result.Add(outputs.FindByType(typeof(Param)) * inputs - sanitize);
result.Add(Find_Source_Equals_Sink(inputs, outputs));

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
result.Add(AngularJS_Find_DOM_XSS());