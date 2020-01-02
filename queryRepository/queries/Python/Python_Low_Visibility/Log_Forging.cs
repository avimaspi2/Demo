CxList inputs = Find_Interactive_Inputs();
CxList logs = Find_Log_Outputs();

CxList sanitize = Find_Integers();

result = logs.InfluencedByAndNotSanitized(inputs, sanitize);