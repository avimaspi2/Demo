CxList methods = Find_Methods();
CxList deadCode = Find_Dead_Code_Contents();

CxList getSession = 
	All.FindByMemberAccess("HttpServletRequest.getSession") +
	All.FindByName("*response.getSession") +  
	All.FindByName("*Response.getSession");

CxList inputs = 
	getSession.GetMembersOfTarget().FindByShortName("getId") +
	All.FindByMemberAccess("HttpSession.getId");

CxList outputs = Find_Log_Outputs();

CxList sanitize = Find_Integers() + deadCode;


result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);