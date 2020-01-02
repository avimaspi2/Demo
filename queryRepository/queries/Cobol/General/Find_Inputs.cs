CxList methodInvokes = Find_Methods();
CxList acceptMethods = methodInvokes.FindByShortName("ACCEPT");
result = All.GetParameters(acceptMethods);
result.Add(Find_Web_Inputs());