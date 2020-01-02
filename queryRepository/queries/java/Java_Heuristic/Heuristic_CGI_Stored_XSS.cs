CxList possibleDb = Find_DB_Heuristic();

if (possibleDb.Count > 0)
{
	CxList outputs = Find_Console_Outputs();
	CxList sanitize = Find_XSS_Sanitize();
	CxList read = Find_Read_NonDB();
	
	CxList possibleDbRead = All.NewCxList();
	possibleDbRead.Add(possibleDb);
	possibleDbRead.Add(read);		

    result = possibleDbRead.InfluencingOnAndNotSanitized(outputs, sanitize - read);
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
	
	if (result.Count > 0)
	{
		CxList db = Find_DB_Out();
		
		CxList dbRead = All.NewCxList();
		dbRead.Add(db);
		dbRead.Add(read);		

		result = Filter_Heuristic_Results(result, dbRead, outputs);
	}
}