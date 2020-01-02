CxList possible_db = Find_DB_Heuristic();

if (possible_db.Count > 0)
{
	CxList db = Find_DB_Base() + Find_Read() - All.FindByShortName("ExecuteNonQuery", false);
	CxList dbParams = All.GetParameters(db);
	CxList sanitize = Find_SQL_Sanitize();
	
	// Find flows: possible_db -> db, possible_db -> possible_db
	result = All.FindSQLInjections(possible_db, possible_db + dbParams, sanitize);
	// Find flows: db -> possible db
	result.Add(All.FindSQLInjections(db, possible_db, sanitize));	
}