if(All.isWebApplication)
{
	CxList possibleDb = Find_DB_Heuristic();

	if (possibleDb.Count > 0)
	{
		CxList outputs = Find_XSS_Outputs();
		CxList sanitize = Find_XSS_Sanitize();
		CxList read = Find_Read_NonDB();
		
		CxList inputs =  All.NewCxList();
		inputs.Add(possibleDb);
		inputs.Add(read);

		result = inputs.InfluencingOnAndNotSanitized(outputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
		result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

		if (result.Count > 0)
		{
				
			CxList dbOutRead = Find_DB_Out();
			dbOutRead.Add(read);

			result = Filter_Heuristic_Results(result, dbOutRead, outputs);
		}
	}
}