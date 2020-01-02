CxList db_out = Find_DB_Out() ;
CxList sanitize = Find_SQL_Sanitize();

CxList db = Find_SQL_DB_In();
CxList dbParams = All.GetParameters(db);

result = All.FindSQLInjections(db_out + Find_Read(), dbParams, sanitize);