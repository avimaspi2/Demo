CxList unknownRefs = Find_Unknown_References();
CxList webRead = unknownRefs.FindByShortName("WEB_SEND");
CxList relevantMethods = webRead.GetAncOfType(typeof(MethodInvokeExpr));

CxList parameters = All.GetParameters(relevantMethods).FindByType(typeof(MethodInvokeExpr));
CxList valueInvokes = parameters.FindByShortName("FROM");
CxList valueInvokesParameters = All.GetParameters(valueInvokes);
	
result = valueInvokesParameters;