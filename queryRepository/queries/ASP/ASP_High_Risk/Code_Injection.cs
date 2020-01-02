List<string> executers = new List<string>(new string[]{"Execute","ExecuteGlobal","eval"});
CxList code = 
	FindByMemberAccess_ASP("Server.Execute") +
	Find_Methods().FindByShortNames(executers, false);
result = Find_Interactive_Inputs().DataInfluencingOn(code);