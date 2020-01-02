CxList db = Find_DB();
CxList db_not_in_try = Improper_Exception_Handling(db);
CxList db_in_try = db - db_not_in_try;

CxList inputs = Find_Read()+Find_DB();
CxList sanitized = Find_Sanitize_SQL_Injection();

result = inputs.InfluencingOnAndNotSanitized(db_in_try, sanitized);