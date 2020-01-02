CxList methodInvokes = Find_Methods();
CxList acceptMethods = methodInvokes.FindByShortName("DISPLAY");
result = All.GetParameters(acceptMethods);
result.Add(Find_Web_Outputs());