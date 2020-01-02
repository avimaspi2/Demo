CxList inputs = Find_Read();
inputs.Add(Find_DB_Out());
inputs -= Find_Headers();

CxList sanitize = Find_Splitting_Sanitizer();

CxList outputs = Find_Header_Outputs();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);