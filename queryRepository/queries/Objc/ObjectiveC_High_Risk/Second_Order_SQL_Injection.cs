CxList dbIn = Find_SQL_DB_In();
CxList dbOut = Find_DB_Out();
CxList sanitize = Find_SQL_Sanitize();
CxList reads = Find_Read();

CxList dbOutRead = All.NewCxList();
dbOutRead.Add(dbOut);	
dbOutRead.Add(reads);

result = dbOutRead.InfluencingOnAndNotSanitized(dbIn, sanitize);