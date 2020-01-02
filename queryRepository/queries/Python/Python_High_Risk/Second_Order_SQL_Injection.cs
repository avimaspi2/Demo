CxList dbOut = Find_DB_Out();
CxList dbIn = Find_SQL_DB_In();

CxList sanitize = Find_Sanitize();
result = dbOut.InfluencingOnAndNotSanitized(dbIn, sanitize);