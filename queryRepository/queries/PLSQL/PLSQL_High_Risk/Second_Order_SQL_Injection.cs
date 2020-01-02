CxList db_input = Find_Dynamic_DB_In();
CxList db_output = Find_DB_Out();
CxList read = Find_Read();
CxList sanitized = Find_Sanitize();

//result = All.FindSQLInjections(db_output + read, db_input, sanitized);
result = db_input.InfluencedByAndNotSanitized((db_output + read), sanitized, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);