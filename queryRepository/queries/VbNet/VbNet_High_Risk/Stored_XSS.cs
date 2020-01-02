if(All.isWebApplication)
{
	CxList db = Find_DB_Out();
	CxList outputs = Find_XSS_Outputs();
	CxList sanitize =  Find_XSS_Sanitize();

	result = All.FindXSS(db + Find_Read(), outputs, sanitize);
}