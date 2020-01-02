CxList psw = Find_Passwords() - Find_Methods();

CxList origin = Find_DB_Out();
origin.Add(Find_Read());

CxList sanitize = Find_Sanitize();

result = origin.InfluencingOnAndNotSanitized(psw, sanitize);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);