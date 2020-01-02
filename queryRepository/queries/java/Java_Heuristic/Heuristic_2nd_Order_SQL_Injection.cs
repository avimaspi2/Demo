CxList possibleDb = Find_DB_Heuristic();

if (possibleDb.Count > 0)
{
	CxList dbOut = Find_DB_Out();
	CxList sanitize = Find_SQL_Sanitize();
	CxList dalDb = Find_DAL_DB();
	CxList read = Find_Read_NonDB();
	
	CxList dbParams = All.GetParameters(possibleDb - dalDb);
	
	CxList dbWithParams = possibleDb.FindByParameters(dbParams);
	CxList dbWithNoParams = possibleDb - dbWithParams;
	
	CxList endDB = All.NewCxList();	
	endDB.Add(dbParams);
	endDB.Add(dbWithNoParams);
	endDB -= dalDb;
	
	CxList dbOutRead = All.NewCxList();
	dbOutRead.Add(dbOut);
	dbOutRead.Add(read);
	
	result = dbOutRead.InfluencingOnAndNotSanitized(endDB, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
	
	if (result.Count > 0)
	{
		CxList dbIn = Find_DB_In();

		CxList dbParamsOrg = All.GetParameters(dbIn - dalDb);
	
		CxList dbWithParamsOrg = dbIn.FindByParameters(dbParams);
		CxList dbWithNoParamsOrg = dbIn - dbWithParamsOrg;
		
		CxList endDBOrg = All.NewCxList();		
		endDB.Add(dbParamsOrg);
		endDB.Add(dbWithNoParamsOrg);
		endDB -= dalDb;

		result = Filter_Heuristic_Results(result, dbOutRead, endDBOrg);
	}
}