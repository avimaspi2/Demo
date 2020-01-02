CxList db = Find_DB_Out();
CxList read = Find_Read_NonDB();
CxList outputs = Find_Console_Outputs();
CxList sanitize = Find_XSS_Sanitize();

result = (db + read).InfluencingOnAndNotSanitized(outputs, sanitize - read).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);