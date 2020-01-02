CxList db = Find_DB_Out();
CxList sanitize = Find_Sanitize();
CxList read = Find_Read();
CxList dbParams = All.GetParameters(Find_DB());

result = All.FindSQLInjections(db + read, dbParams, sanitize);