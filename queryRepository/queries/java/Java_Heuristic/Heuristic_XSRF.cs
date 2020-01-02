if (All.isWebApplication)
{	
	CxList possibleDb = Find_DB_Heuristic();

	if (possibleDb.Count > 0)
	{
		CxList requests = Find_Interactive_Inputs();
		requests.Add(All.FindByMemberAccess("HttpServletRequest.getQueryString"));
		requests.Add(All.FindByName("*request.getQueryString"));
		requests.Add(All.FindByName("*Request.getQueryString"));
		
		CxList java_xsrf_sanitize = Find_XSRF_Sanitize();
		requests -= requests.GetByAncs(java_xsrf_sanitize);
		
		CxList strings = Find_Strings();
		
		CxList write = strings.FindByName("*update*", StringComparison.OrdinalIgnoreCase);
		write.Add(strings.FindByName("*delete*", StringComparison.OrdinalIgnoreCase));
		write.Add(strings.FindByName("*insert*", StringComparison.OrdinalIgnoreCase));

		CxList dbWrite = possibleDb.DataInfluencedBy(write);
		dbWrite.Add(possibleDb.FindByShortName("update*"));
		dbWrite.Add(possibleDb.FindByShortName("delete*"));
		dbWrite.Add(possibleDb.FindByShortName("insert*"));
		
	
		result = dbWrite.DataInfluencedBy(requests);		
		result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

		if (result.Count > 0)
		{
			CxList db = Find_DB_In();
			
			CxList dbWriteOrg = db.DataInfluencedBy(write);
			dbWriteOrg.Add(db.FindByShortName("update*"));
			dbWriteOrg.Add(db.FindByShortName("delete*"));
			dbWriteOrg.Add(db.FindByShortName("insert*"));
			
			result = Filter_Heuristic_Results(result, requests, dbWriteOrg);
		}
	}
}