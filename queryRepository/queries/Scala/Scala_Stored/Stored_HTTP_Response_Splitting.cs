CxList sanitize = Find_Splitting_Sanitizer();
CxList inputs = Find_Read() + Find_DB_Out() - Find_Headers();
CxList outputs = Find_Header_Outputs();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);