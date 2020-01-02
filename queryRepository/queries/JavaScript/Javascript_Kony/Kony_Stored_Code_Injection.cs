/*
	This query finds flows from stored data to JS eval
*/
if(cxScan.IsFrameworkActive("KonyInFF"))
{
	// extract stored data from the database
	CxList inputs = Kony_DataStore_Inputs();
	inputs.Add(Kony_LocalStore_Inputs());
	inputs.Add(Kony_DB_Out());
	inputs.Add(Kony_FileSystem_Inputs());
	
	CxList Eval = Find_Outputs_CodeInjection();
	CxList sanitize = Code_Injection_Sanitize();
	
	result.Add(Eval.FindByType(typeof(Param)) * inputs - sanitize);
	
	result.Add(Eval.InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm));
	result.Add(Find_Source_Equals_Sink(inputs, Eval));
}