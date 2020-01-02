CxList db = general.Find_SQL_DB_In();
CxList inputs = general.Find_Interactive_Inputs();
CxList sanitized = general.Find_SQL_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(db, sanitized);