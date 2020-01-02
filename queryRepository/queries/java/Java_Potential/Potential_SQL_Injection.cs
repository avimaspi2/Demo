CxList db = Find_SQL_DB_In();
CxList inputs = Find_Potential_Inputs();
CxList sanitized = Find_SQL_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(db, sanitized);