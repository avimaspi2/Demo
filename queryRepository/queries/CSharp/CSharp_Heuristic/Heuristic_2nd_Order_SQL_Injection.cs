CxList possible_db = Find_DB_Heuristic();
possible_db -= possible_db.DataInfluencedBy(possible_db);

if (possible_db.Count > 0)
{
	CxList db = Find_DB_Base() + Find_Read();
	CxList dbParams = All.GetParameters(db);
	CxList sanitize = Find_SQL_Sanitize();
	result = possible_db.InfluencingOnAndNotSanitized(possible_db + dbParams, sanitize);
	result.Add(db.InfluencingOnAndNotSanitized(possible_db, sanitize));
}