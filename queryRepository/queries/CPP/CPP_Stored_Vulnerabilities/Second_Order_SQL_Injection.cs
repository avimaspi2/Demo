CxList db = Find_DB();
CxList sanitize = Find_Sanitize_SQL_Injection();

result = All.FindSQLInjections(Find_Read()+db, db, sanitize);