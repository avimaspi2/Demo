if(Lightning_Find_Aura_Cmp_And_App().Count > 0)
{     
	CxList dbOut = Lightning_Find_SOQL_DB_Out_SS();
	CxList outputs = Lightning_Find_Outputs_XSS();
	CxList sanitize = Sanitize();
	sanitize.Add(Find_XSS_Sanitize());
	sanitize.Add(Lightning_XSS_Sanitizer());
	result = outputs.InfluencedByAndNotSanitized(dbOut, sanitize);
}