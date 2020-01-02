CxList outputs = NodeJS_Find_DB_IN();
CxList inputs = NodeJS_Find_DB_Out();
CxList sanitize = NodeJS_Find_Sanitize();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);