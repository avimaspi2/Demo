CxList unknownRefs = Find_Unknown_References();
CxList read = unknownRefs.FindByShortName("READQ*");
CxList relevantMethods = read.GetAncOfType(typeof(MethodInvokeExpr));

CxList parameters = All.GetParameters(relevantMethods).FindByType(typeof(MethodInvokeExpr));
CxList queueInvokes = parameters.FindByShortName("QUEUE");
CxList queueInvokesParameters = All.GetParameters(queueInvokes);
	
result = queueInvokesParameters;