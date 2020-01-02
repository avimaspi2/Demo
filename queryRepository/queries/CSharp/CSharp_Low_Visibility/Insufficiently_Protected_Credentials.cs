CxList psw  = Find_Passwords();

psw = psw - Find_Methods();

CxList DB = All.FindByName("*db*") + Find_DB_Out();

result = psw.DataInfluencedBy(DB);

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);