CxList setSessionAttr = Set_Session_Attribute();
CxList setContextAttr = Set_Context_Attribute();

CxList sessionSanitize = Find_General_Sanitize() + 
	All.FindByType("HttpSession") + 
	All.FindByShortName("getSession");
CxList contextSanitize = Find_General_Sanitize() + 
	All.FindByType("ServletContext") + 
	All.FindByShortName("getServletContext");

CxList input = Find_Inputs();

CxList secondParamSessionAttr = All.GetParameters(setSessionAttr, 1);
CxList secondParamContextAttr = All.GetParameters(setContextAttr, 1);
result = 
	secondParamSessionAttr.InfluencedByAndNotSanitized(input, sessionSanitize) +
	secondParamContextAttr.InfluencedByAndNotSanitized(input, contextSanitize);