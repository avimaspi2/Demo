if (CGI().Count > 0) //web application (CGI)
{
	CxList possible_db = Find_DB_Heuristic();

	if (possible_db.Count > 0)
	{
		CxList sanitize = Find_Sanitize() + All.FindByShortName("encode", false);
		CxList stored = possible_db + Find_Read();
		result = stored.InfluencingOnAndNotSanitized(Find_Outputs(), sanitize);
	
		if (result.Count > 0)
		{
			CxList db = Find_DB();
			stored = db + Find_Read();
			result -= stored.InfluencingOnAndNotSanitized(Find_Outputs(), sanitize);
		}
	}
}