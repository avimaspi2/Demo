CxList psw  = Find_Passwords();

psw = psw - Find_Methods();

CxList DB = All.FindByName("*db*") + Find_DB();

result = psw.DataInfluencedBy(DB);