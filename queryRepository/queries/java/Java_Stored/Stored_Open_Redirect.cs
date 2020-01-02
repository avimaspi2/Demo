CxList inputs = Find_Read();
inputs.Add(Find_DB_Out());

CxList sanitize = Find_Integers();

CxList redirect = Find_Redirects();

result = redirect.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);