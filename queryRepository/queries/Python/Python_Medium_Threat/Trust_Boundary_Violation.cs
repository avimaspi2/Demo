CxList inputs = Find_Inputs();
CxList session = Find_Session();

CxList sanitizers = Find_DB_In();
sanitizers.Add(Find_Read());

result = session.InfluencedByAndNotSanitized(inputs, sanitizers, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);