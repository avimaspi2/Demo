CxList possible_db = Find_DB_Heuristic();
possible_db -= possible_db.DataInfluencedBy(possible_db);

if (possible_db.Count > 0)
{
	CxList inputs = Find_Interactive_Inputs();
	CxList sanitized = Find_SQL_Sanitize();

	result = inputs.InfluencingOnAndNotSanitized(possible_db, sanitized);
}