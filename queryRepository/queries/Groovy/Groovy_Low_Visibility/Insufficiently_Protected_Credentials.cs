CxList psw = Find_Passwords();
psw -= Find_Methods();

CxList DB = Find_DB_Out();
CxList sanitize = Find_General_Sanitize();

result = DB.InfluencingOnAndNotSanitized(psw, sanitize);