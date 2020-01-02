CxList possible_db = Find_DB_Heuristic();

if (possible_db.Count > 0)
{
	CxList db = Find_DB();
	CxList sanitize = Find_Sanitize_SQL_Injection();
	
	result = (Find_Read() + possible_db).InfluencingOnAndNotSanitized(possible_db, sanitize);
	
	if (result.Count > 0)
	{
		result -= (Find_Read() + db).InfluencingOnAndNotSanitized(db, sanitize);
	}
}