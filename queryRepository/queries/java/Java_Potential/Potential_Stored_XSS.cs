CxList db = Find_DB_Out();
CxList read = Find_Read_NonDB();
CxList outputs = Find_Potential_Outputs() - Find_Header_Outputs();
CxList sanitize = Find_XSS_Sanitize();

CxList dbRead = All.NewCxList();
dbRead.Add(db);
dbRead.Add(read);

result = dbRead.InfluencingOnAndNotSanitized(outputs, sanitize);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);