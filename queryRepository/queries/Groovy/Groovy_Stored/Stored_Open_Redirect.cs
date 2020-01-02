CxList redirect = Find_Redirects();
CxList inputs = Find_Read() + Find_DB_Out();
CxList sanitize = Find_Integers();

result = redirect.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);