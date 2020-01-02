CxList db_out = Find_DB_Out() + Find_Read();
CxList db_in = Find_DB_In();
CxList sanitize = Find_Sanitize();

result = db_out.InfluencingOnAndNotSanitized(db_in, sanitize);