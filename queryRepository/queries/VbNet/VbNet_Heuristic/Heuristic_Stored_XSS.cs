if(All.isWebApplication)
{
	CxList possible_db = Find_DB_Heuristic() - Find_IO();

	if (possible_db.Count > 0)
	{
		CxList outputs = Find_XSS_Outputs();
		CxList sanitize = Find_XSS_Sanitize();

		result = All.FindXSS(possible_db, outputs, sanitize);

	}
}