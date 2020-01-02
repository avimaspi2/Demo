CxList sapWrapper = Find_SAP_Library().GetMembersOfTarget().FindByShortNames(new List<string> {"sjax", "syncGet", "syncGetJSON", "syncPost", "syncGetText"});
result.Add(sapWrapper);

CxList xhrWrappers = Find_XHR_Wrappers();
CxList fieldDeclsList = Find_FieldDecls();
CxList associativeArrayList = Find_AssociativeArrayExpr();
CxList xhrWrappersFieldsList = fieldDeclsList.FindByFathers(associativeArrayList.GetParameters(xhrWrappers));
List<string> methodNames = new List<string> {"done", "fail", "always", "success", "error", "complete"};
CxList xhrCallbacks = xhrWrappersFieldsList.FindByShortNames(methodNames).GetAssigner();
result.Add(All.GetParameters(xhrCallbacks, 0));

xhrCallbacks = xhrWrappers.FindByShortNames(methodNames);
CxList lambdaExprs = Find_LambdaExpr().GetParameters(xhrCallbacks, 0);
result.Add(Find_ParamDecl().GetParameters(lambdaExprs, 0));

CxList xhrOpen = Find_XmlHttp_Open().GetTargetOfMembers();
CxList xhrRef = All.FindAllReferences(xhrOpen);

CxList wrappersAssignee = xhrWrappers.GetAssignee();
wrappersAssignee.Add(xhrWrappers);

xhrRef.Add(All.FindAllReferences(wrappersAssignee));
result.Add(xhrRef.GetMembersOfTarget().FindByShortNames(new List<string> {"response", "responseText"}));

result.Add(Find_JQuery_Ajax_Inputs());