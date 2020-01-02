CxList dbOut = Find_DB_Out();

CxList read = All.NewCxList();
read.Add(Find_Read_NonDB());
read.Add(Find_FileSystem_Read());

CxList dbIn = Find_SQL_DB_In();

CxList sanitized = Find_SQL_Sanitize();

CxList dbParams = All.GetParameters(dbIn);

CxList dbWithParams = dbIn.FindByParameters(dbParams);
CxList dbWithNoParams = dbIn - dbWithParams;

CxList endDB = All.NewCxList();
endDB.Add(dbParams);
endDB.Add(dbWithNoParams);

CxList dbOutRead = All.NewCxList();
dbOutRead.Add(dbOut);
dbOutRead.Add(read);

result = dbOutRead.InfluencingOnAndNotSanitized(endDB, sanitized, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);