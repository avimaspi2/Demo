CxList db = Find_SQL_DB_In();
CxList db_not_in_try = Improper_Exception_Handling(db);
CxList db_in_try = db - db_not_in_try;

CxList inputs = Find_Interactive_Inputs();
CxList sanitized = Find_SQL_Sanitize();

result = All.FindSQLInjections(inputs, db_in_try, sanitized);