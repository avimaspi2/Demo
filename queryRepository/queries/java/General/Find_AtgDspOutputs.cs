CxList AllDsp = Get_AllDSP();
if (AllDsp.Count > 0)
{
	// Query parameters
	CxList outputs = Find_Interactive_Outputs();
	outputs.Add(Find_Write());
	outputs.Add(Find_Html_Outputs());
	outputs.Add(Find_Log_Outputs());

	CxList specialPaths = Find_AtgDspSetGet();
	int maxIter = 4; // Maximun amount of partial paths connected by set-get paths and lead to outputs.

	// Query body
	CxList partialResults = All.NewCxList();
	partialResults.Add(outputs);
	
	int prevResultsCount = 0;
	int currResultsCount = partialResults.pathData.Count + partialResults.Count;
	int i = 0;

	while (i < maxIter && prevResultsCount < currResultsCount)
	{
		i++;
		prevResultsCount = currResultsCount;

		partialResults = specialPaths.DataInfluencingOn(partialResults, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
		currResultsCount = partialResults.pathData.Count + partialResults.Count;
	}

	result = partialResults;
}