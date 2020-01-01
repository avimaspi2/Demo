CxList possible_db = Find_DB_Heuristic();

if (possible_db.Count > 0)
{
	CxList outputs = Find_XSS_Outputs();
	CxList sanitize = Find_XSS_Sanitize() - Find_LDAP();

	result = All.FindXSS(possible_db + Find_IO(), outputs, sanitize);

	if (result.Count > 0)
	{
		CxList db = Find_DB_Out();
		result -= All.FindXSS(db + Find_IO(), outputs, sanitize);
	}
}