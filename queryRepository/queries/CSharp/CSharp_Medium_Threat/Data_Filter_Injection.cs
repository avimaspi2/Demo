// Data Filter Manipulation (Information Leak)

CxList db = Find_Disconnected_DB_Access();
CxList inputs = Find_Interactive_Inputs();
CxList sanitized = Find_SQL_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(db, sanitized);