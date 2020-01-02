// Data Filter Manipulation (Information Leak)

CxList db = Find_Disconnected_DB_Access();
CxList inputs = Find_Interactive_Inputs();
CxList sanitized = Find_Sanitize();

result = All.FindSQLInjections(inputs, db, sanitized);