CxList setSessionAttr = Set_Session_Attribute();
CxList setContextAttr = Set_Context_Attribute();
CxList generalSanitize = Find_General_Sanitize();

CxList sessionSanitize = All.NewCxList(); 
sessionSanitize.Add(generalSanitize); 
sessionSanitize.Add(All.FindByType("HttpSession")); 
sessionSanitize.Add(All.FindByShortName("getSession"));

CxList contextSanitize = All.NewCxList();
contextSanitize.Add(generalSanitize); 
contextSanitize.Add(All.FindByType("ServletContext")); 
contextSanitize.Add(All.FindByShortName("getServletContext"));

CxList input = Find_DB_Out();

CxList secondParamSessionAttr = All.GetParameters(setSessionAttr, 1);
CxList secondParamContextAttr = All.GetParameters(setContextAttr, 1);

result = secondParamSessionAttr.InfluencedByAndNotSanitized(input, sessionSanitize);
result.Add(secondParamContextAttr.InfluencedByAndNotSanitized(input, contextSanitize));

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);