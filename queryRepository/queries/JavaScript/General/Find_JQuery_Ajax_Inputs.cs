//Get the return values from an Ajax request

CxList methods = Find_JQuery_Methods();
CxList unknownRef = Find_UnknownReference();
CxList fieldDecls = Find_FieldDecls();
CxList methodDecls = Find_MethodDecls();
CxList paramDecls = Find_ParamDecl();
CxList members = Find_MemberAccesses();
CxList JQueryAjax = methods.FindByShortName("ajax");

//Get all success or error params from $.ajax:({ success: func(x), error: func(y)})
CxList AjaxInputMtd = fieldDecls.FindByShortNames(new List<string> {"success", "error"}).GetByAncs(JQueryAjax);

//Get all success or error params from methods called by a unkwnownRef param in $.ajax
CxList AjaxAnonyVar = unknownRef.GetParameters(JQueryAjax, 0);
CxList membersVar = unknownRef.FindAllReferences(AjaxAnonyVar);
CxList assignedTo = members.FindByShortNames(new List<string> {"success", "error"}).FindByFathers(membersVar);
CxList assignedMethod = methodDecls.FindAllReferences(assignedTo.GetAssigner());

//Get all success params from $.post( url [, data ] [, success function ] [, dataType ] )
CxList jQueryGet = methods.FindByShortNames(new List<string> {"get", "post", "getJSON", "getScript"});
CxList lambdas = Find_LambdaExpr();
CxList relevantParameters = unknownRef + members + lambdas;
CxList jQueryGetParams = relevantParameters.GetParameters(jQueryGet, 1);
jQueryGetParams.Add(relevantParameters.GetParameters(jQueryGet, 2));

// Get definitions of jQuery Params and member access
CxList successMethods = methodDecls.FindAllReferences(jQueryGetParams);
try {
	CxList dummy = jQueryGetParams.FindByAbstractValue(v => {
		if (v is FunctionAbstractValue) {
			FunctionAbstractValue f = v as FunctionAbstractValue;
			foreach (var definitionId in f.Definitions) {
				IAbstractValue absValue = new FunctionAbstractValue(definitionId);
				CxList b = lambdas.FindByAbstractValue(abstractValue => absValue.Contains(abstractValue));
				successMethods.Add(b);
			}
		}
		return false;
		});
} catch (Exception e) {
	cxLog.WriteDebugMessage(e);
}

result = paramDecls.GetParameters(methodDecls.GetByAncs(AjaxInputMtd), 0);
result.Add(paramDecls.GetParameters(assignedMethod, 0));
result.Add(paramDecls.GetParameters(successMethods, 0));