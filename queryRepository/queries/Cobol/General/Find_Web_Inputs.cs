CxList unknownRefs = Find_Unknown_References();
CxList webRead = unknownRefs.FindByShortName("WEB_READ");
CxList relevantMethods = webRead.GetAncOfType(typeof(MethodInvokeExpr));

CxList parameters = All.GetParameters(relevantMethods).FindByType(typeof(MethodInvokeExpr));
CxList valueInvokes = parameters.FindByShortName("VALUE");
CxList valueInvokesParameters = All.GetParameters(valueInvokes);
	
result = valueInvokesParameters;