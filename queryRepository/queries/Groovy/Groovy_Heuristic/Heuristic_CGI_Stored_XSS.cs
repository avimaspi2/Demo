CxList possible_db = Find_DB_Heuristic();

if (possible_db.Count > 0)
{
	CxList outputs = Find_Console_Outputs();
	CxList sanitize = Find_XSS_Sanitize();

	result = All.FindXSS(possible_db + Find_Read(), outputs, sanitize - Find_Read());

	if (result.Count > 0)
	{
		CxList db = Find_DB_Out();
		result -= All.FindXSS(db + Find_Read(), outputs, sanitize - Find_Read());
	}
}