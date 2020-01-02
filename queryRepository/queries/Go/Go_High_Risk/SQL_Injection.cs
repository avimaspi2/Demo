CxList inputPaths = Find_Inputs();

CxList dbPaths = Find_DB_Out();
dbPaths.Add(Find_DB_In());

CxList sanitizers = Find_DB_Sanitize();

result = inputPaths.InfluencingOnAndNotSanitized(dbPaths, sanitizers);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);