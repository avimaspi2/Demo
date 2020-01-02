CxList header_inputs = Find_Headers();

CxList sanitize = Find_Splitting_Sanitizer();

CxList inputs = Find_Read() + Find_DB_Out() - header_inputs;

CxList outputs = Find_Header_Outputs();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);