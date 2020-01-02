CxList dbOut = Find_DB_Out() + Find_Read() + Find_Session_Output();
CxList dbIn = Find_DB_In();
CxList sanitize = Find_SQL_Injection_Sanitize();

result = dbOut.InfluencingOnAndNotSanitized(dbIn, sanitize);