CxList dbOut = Find_DB_Out();
CxList read = Find_Read_NonDB();
CxList dbIn = Find_SQL_DB_In();

CxList sanitized = Find_SQL_Sanitize();

CxList dbParams = All.GetParameters(dbIn);

CxList dbWithParams = dbIn.FindByParameters(dbParams);
CxList dbWithNoParams = dbIn - dbWithParams;
CxList endDB = dbParams + dbWithNoParams;

result = (dbOut + read).InfluencingOnAndNotSanitized(endDB, sanitized, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);