if(All.isWebApplication)
{
	CxList possible_db = Find_DB_Heuristic();

	if (possible_db.Count > 0)
	{
		CxList outputs = Find_XSS_Outputs();
		CxList sanitize = Find_XSS_Sanitize();

		result = (possible_db + Find_IO()).InfluencingOnAndNotSanitized(outputs, sanitize)
			.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

		if (result.Count > 0)
		{
			CxList db = Find_DB_Out();
			result -= (db + Find_IO()).InfluencingOnAndNotSanitized(outputs, sanitize);
		}
	}
}