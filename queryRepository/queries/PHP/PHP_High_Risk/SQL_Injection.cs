CxList dbIn = Find_DB_In();
CxList inputs = Find_Interactive_Inputs();
CxList sanitized = Find_SQL_Injection_Sanitize();

CxList sqlInjections = dbIn.InfluencedByAndNotSanitized(inputs, sanitized);
result = sqlInjections.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);