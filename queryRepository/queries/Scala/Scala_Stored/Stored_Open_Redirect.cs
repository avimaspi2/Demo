CxList redirects = Find_Redirects();
CxList inputs = Find_Read() + Find_DB_Out();
CxList sanitize = Find_Integers();

result = redirects.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);