if(cxScan.IsFrameworkActive("AngularJS"))
{
	CxList cookiesStringLiterals = Find_String_Literal().FindByShortName("$cookies");
	CxList methodDecls = Find_MethodDecls();
	CxList paramDecls = Find_ParamDecl();
	CxList unknownReferences = Find_UnknownReference();

	foreach(CxList cookieString in cookiesStringLiterals) {
		ArrayInitializer arrayInit = cookieString.GetFathers().FindByType(typeof(ArrayInitializer))
			.TryGetCSharpGraph<ArrayInitializer>();
		StringLiteral cookieLiteral = cookieString.TryGetCSharpGraph<StringLiteral>();
	
		if(arrayInit != null && arrayInit.InitialValues != null && cookieLiteral != null)
		{
			int position = arrayInit.InitialValues.IndexOf(cookieLiteral);
			Expression e = arrayInit.InitialValues[arrayInit.InitialValues.Count - 1];
			CxList func = All.NewCxList();
			if(e is LambdaExpr) {
				func.Add(All.FindById(e.NodeId));
			} else if(e is UnknownReference) {
				CxList unknownReference = All.FindById(e.NodeId);
				unknownReference = unknownReference.FindByAbstractValue(aV => aV is FunctionAbstractValue);
				func.Add(unknownReferences.FindAllReferences(unknownReference).GetAssigner());
				func.Add(methodDecls.FindDefinition(unknownReference));
			}
			CxList parameter = paramDecls.GetParameters(func, position);
			result.Add(parameter);
			result.Add(unknownReferences.GetByAncs(func).FindAllReferences(parameter));
		}
	}
}