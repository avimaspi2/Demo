CxList methods = Find_Methods();
CxList deadCode = Find_Dead_Code_Contents();

CxList getSession = 
	All.FindByMemberAccess("HttpServletRequest.getSession") +
	All.FindByName("*response.getSession") +  
	All.FindByName("*Response.getSession");

CxList inputs = 
	getSession.GetMembersOfTarget().FindByShortName("getId") +
	All.FindByMemberAccess("HttpSession.getId");

CxList outputs = methods.FindByName("log", false);

CxList sanitize = Find_Integers() + deadCode;


result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);