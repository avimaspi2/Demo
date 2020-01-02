if(All.isWebApplication)
{
	CxList possible_db = Find_DB_Heuristic();
	possible_db -= possible_db.DataInfluencedBy(possible_db);
	possible_db -= Find_Read();

	if (possible_db.Count > 0)
	{
		CxList outputs = Find_XSS_Outputs();
		CxList sanitize = Find_XSS_Sanitize();

		result = (possible_db).InfluencingOnAndNotSanitized(outputs, sanitize);
	}
}