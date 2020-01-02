CxList db = Find_DB_Out();
CxList read = Find_Read_NonDB();
CxList outputs = Find_Potential_Outputs() - Find_Header_Outputs();
CxList sanitize = Find_XSS_Sanitize();

result = (db + read).InfluencingOnAndNotSanitized(outputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);