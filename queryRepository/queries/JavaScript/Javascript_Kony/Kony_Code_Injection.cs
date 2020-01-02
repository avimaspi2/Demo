/*
	This query finds flosw from user input to JS eval
*/
if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList inputs = Kony_UI_Inputs();
	CxList Eval = Find_Outputs_CodeInjection();
	CxList sanitize = Code_Injection_Sanitize();
	
	result.Add(Eval.FindByType(typeof(Param)) * inputs - sanitize);
	
	result.Add((Eval).InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm));
	result.Add(Find_Source_Equals_Sink(inputs, Eval));
}