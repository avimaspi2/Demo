CxList possible_db = Find_DB_Heuristic();

if (possible_db.Count > 0)
{
	CxList dbOut = Find_DB_Out();
	CxList dbIn = Find_SQL_DB_In();
	CxList sanitize = Find_SQL_Sanitize();
	
	result = All.FindSQLInjections(Find_Read() + possible_db, possible_db - Find_DAL_DB(), sanitize);
	
	if (result.Count > 0)
	{
		result -= All.FindSQLInjections(Find_Read() + dbOut, dbIn, sanitize);
	}
}