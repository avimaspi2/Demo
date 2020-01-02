CxList db_out = Find_DB_Out();
CxList db_in = Find_SQL_DB_In();

CxList sanitize = Find_SQL_Sanitize();
result=db_out.InfluencingOnAndNotSanitized(db_in,sanitize);