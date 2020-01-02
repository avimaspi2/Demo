CxList setSessionAttr = Set_Session_Attribute(); 
CxList setContextAttr = Set_Context_Attribute();
CxList generalSanitize = Find_General_Sanitize();
CxList input = Find_Interactive_Inputs();

CxList sessionSanitize = Trust_Boundary_Violation_Session_Sanitize();
sessionSanitize.Add(generalSanitize);
	
CxList contextSanitize =  Trust_Boundary_Violation_Context_Sanitize();
contextSanitize.Add(generalSanitize);

CxList ParamSessionAttr = All.GetParameters(setSessionAttr);
CxList ParamContextAttr = All.GetParameters(setContextAttr);

CxList paramSessionAttr = ParamSessionAttr.InfluencedByAndNotSanitized(input, sessionSanitize);
paramSessionAttr.Add(ParamContextAttr.InfluencedByAndNotSanitized(input, contextSanitize));

result = paramSessionAttr.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);