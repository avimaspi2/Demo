CxList possible_db = Find_DB_Heuristic();

if (possible_db.Count > 0)
{
	CxList inputs = Find_Interactive_Inputs();
	CxList sanitized = Find_Sanitize_SQL_Injection();

	result = inputs.InfluencingOnAndNotSanitized(possible_db, sanitized);

	if (result.Count > 0)
	{
		CxList db = Find_DB();
		result -= inputs.InfluencingOnAndNotSanitized(db, sanitized);
	}
}