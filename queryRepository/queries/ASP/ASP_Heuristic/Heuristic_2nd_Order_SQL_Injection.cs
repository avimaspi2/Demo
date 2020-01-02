CxList possible_db = Find_DB_Heuristic();

if (possible_db.Count > 0)
{
	CxList db = Find_SQL_DB_In();
	CxList sanitize = Find_SQL_Sanitize();
	result = All.FindSQLInjections(Find_Read() + possible_db, possible_db, sanitize);
	if (result.Count > 0)
	{
		result = All.FindSQLInjections(Find_Read() + Find_DB_Out(), db, sanitize);
	}
}