CxList possibleDb = Find_DB_Heuristic();

if (possibleDb.Count > 0)
{
	CxList inputs = Find_Interactive_Inputs();
	CxList sanitized = Find_SQL_Sanitize();
	CxList DalDB = Find_DAL_DB();

	result = inputs.InfluencingOnAndNotSanitized(possibleDb - DalDB , sanitized, CxList.InfluenceAlgorithmCalculation.NewAlgorithm); 

	if (result.Count > 0)
	{
		CxList db = Find_DB_In();
		db.Add(All.FindByMemberAccess("String.*"));
		db.Add(All.FindByMemberAccess("StringBuffer.*"));
		
		result = Filter_Heuristic_Results(result, inputs, db - DalDB);
	}
}