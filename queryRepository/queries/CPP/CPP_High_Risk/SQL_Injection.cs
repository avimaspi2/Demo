CxList db = Find_DB();
CxList inputs = Find_Interactive_Inputs();
CxList sanitized = Find_Sanitize_SQL_Injection();

result = inputs.InfluencingOnAndNotSanitized(db, sanitized);