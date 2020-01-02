CxList listenAndServe = Find_Methods().FindByShortName("ListenAndServe",false);
CxList listenSecondParam = All.GetParameters(listenAndServe,1);

CxList methodInvokes = listenSecondParam.FindByType(typeof(MethodInvokeExpr));

CxList hstsInMiddleware = All.GetByAncs(All.FindDefinition(methodInvokes)).FindByShortName("Strict-Transport-Security").GetByAncs(Find_Change_Response_Header());

result = hstsInMiddleware.GetAncOfType(typeof(MethodInvokeExpr));