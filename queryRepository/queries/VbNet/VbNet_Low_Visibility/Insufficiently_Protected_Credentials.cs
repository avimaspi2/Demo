CxList psw  = Find_Passwords();

psw = psw - Find_Methods();

CxList DB = All.FindByName("*db*",false);
DB.Add(Find_DB_Out());

result = psw.DataInfluencedBy(DB);