CxList methods = Find_Methods();

CxList redirect = methods.FindByMemberAccess("HttpResponse.Redirect");
redirect.Add(methods.FindByName("*Response.Redirect"));

CxList inputs = All.FindByMemberAccess("*HttpRequest.QueryString_*");
inputs.Add(All.FindByName("*Request.QueryString_*"));
inputs.Add(All.FindByShortName("Request").FindByFathers(Find_IndexerRefs()));
CxList sanitize = Find_Sanitize();

CxList islocalUrlSanitizer = methods.FindByMemberAccess("Url.IsLocalUrl");
CxList references = Find_Unknown_References();
CxList sanitizedParams = references.GetParameters(islocalUrlSanitizer);
// We're assuming that if users test an input string with "Url.IsLocalUrl(input)" every reference of that input is sanitized.
CxList paramReferences = references.FindAllReferences(sanitizedParams);
foreach(CxList isLocalUrl in islocalUrlSanitizer){
	isLocalUrl.Add(references.FindAllReferences(isLocalUrl.GetAssignee()));
	CxList ifParent = isLocalUrl.GetFathers().FindByType(typeof(IfStmt));
	if(ifParent.Count > 0){
		CxList trueBlock = ifParent.GetBlocksOfIfStatements(true);
		sanitize.Add(paramReferences.GetByAncs(trueBlock));
		continue;
	}
	CxList notParentThenIf = isLocalUrl.GetFathers().FindByType(typeof(UnaryExpr))
		.FindByShortName("Not").GetFathers().FindByType(typeof(IfStmt));
	if(notParentThenIf.Count > 0){
		CxList falseBlock = notParentThenIf.GetBlocksOfIfStatements(false);
		sanitize.Add(paramReferences.GetByAncs(falseBlock));
		continue;
	}
	CxList ternaryExpr = isLocalUrl.GetFathers().FindByType(typeof(TernaryExpr));
	if(ternaryExpr.Count > 0){
		TernaryExpr ternaryExprDom = ternaryExpr.TryGetCSharpGraph<TernaryExpr>();
		
		CxList trueExpression = All.NewCxList();
		trueExpression.Add(ternaryExprDom.True.NodeId, ternaryExprDom.True);
		sanitize.Add(paramReferences.GetByAncs(trueExpression));
		continue;
	}
	CxList ternaryExprFalse = isLocalUrl.GetFathers().FindByType(typeof(UnaryExpr))
		.FindByShortName("Not").GetFathers().FindByType(typeof(TernaryExpr));
	if(ternaryExprFalse.Count > 0){
		TernaryExpr ternaryExprFalseDom = ternaryExprFalse.TryGetCSharpGraph<TernaryExpr>();
		
		CxList falseExpression = All.NewCxList();
		falseExpression.Add(ternaryExprFalseDom.False.NodeId, ternaryExprFalseDom.False);
		sanitize.Add(paramReferences.GetByAncs(falseExpression));
		continue;
	}
}

result = redirect.InfluencedByAndNotSanitized(inputs, sanitize);

CxList aspNetControllers = Find_Classes().InheritsFrom("Controller");
CxList controllerRedirects = methods.FindByShortNames(new List<string>{"Redirect", "RedirectToAction", "RedirectToRoute"})
	.GetByAncs(aspNetControllers);

result.Add(controllerRedirects.InfluencedByAndNotSanitized(Find_Interactive_Inputs(), sanitize));