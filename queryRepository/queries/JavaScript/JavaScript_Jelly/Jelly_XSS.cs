if(cxScan.IsFrameworkActive("Jelly"))
{
	CxList methods = Find_Methods(); 	
	CxList outputs = Find_Outputs_XSS(); 
	CxList inputs  = Find_Inputs_NoWindowLocation();
	
	CxList jellyOutputs = All.GetParameters(methods.FindByShortName("CxOutput")); 
	CxList jellyInputs  = All.GetParameters(methods.FindByShortName("CxInput"));
	
	CxList sanitize = Sanitize();
	sanitize.Add(Find_XSS_Sanitize());
	sanitize.Add(Jelly_Find_XSS_Sanitize());

	CxList allOutputs = All.NewCxList();
	allOutputs.Add(outputs);
	allOutputs.Add(jellyOutputs);
	
	result = allOutputs.InfluencedByAndNotSanitized(jellyInputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
	result.Add(jellyOutputs.InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm));
	
	result.Add(Find_Source_Equals_Sink(jellyInputs, jellyOutputs));

	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}