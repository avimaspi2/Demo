CxList possible_db = Find_DB_Heuristic();

if (possible_db.Count > 0)
{
	CxList inputs = Find_Interactive_Inputs();
	CxList sanitized = Find_SQL_Sanitize();

	result = inputs.InfluencingOnAndNotSanitized(possible_db, sanitized);

	if (result.Count > 0)
	{
		CxList db = Find_SQL_DB_In();
		result -= inputs.InfluencingOnAndNotSanitized(db, sanitized);
	}
}