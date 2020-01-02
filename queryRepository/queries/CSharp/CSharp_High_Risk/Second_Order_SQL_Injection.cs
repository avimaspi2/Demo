CxList db = Find_DB_Out() ;
CxList sanitize = Find_SQL_Sanitize();

CxList dbParams = Find_DB_In();
CxList dbAndRead = db + Find_Read() - All.FindByShortName("ExecuteNonQuery");

result = dbAndRead.InfluencingOnAndNotSanitized(dbParams, sanitize);