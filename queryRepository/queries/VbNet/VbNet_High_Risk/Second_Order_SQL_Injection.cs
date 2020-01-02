CxList db = Find_DB_Out();
CxList sanitize = Find_Sanitize();

CxList dbParams = Find_DB_In();

result = All.FindSQLInjections(db + Find_Read() - All.FindByShortName("ExecuteNonQuery", false), 
	dbParams, sanitize);