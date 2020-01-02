CxList inputs = Find_Inputs();
CxList methodInvokes = Find_Methods();
CxList callMethods = methodInvokes.FindByShortName("CALL");
CxList parameters = All.GetParameters(callMethods);

result = inputs.DataInfluencingOn(parameters);