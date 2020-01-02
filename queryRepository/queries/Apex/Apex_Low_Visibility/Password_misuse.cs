CxList psw  = Find_Passwords();
psw -= Find_Methods();
CxList DB = Find_DB_Input();
result = DB.InfluencingOnAndNotSanitized(psw, Find_Test_Code());

result -= Find_Test_Code();
result -= result.DataInfluencedBy(result);

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);