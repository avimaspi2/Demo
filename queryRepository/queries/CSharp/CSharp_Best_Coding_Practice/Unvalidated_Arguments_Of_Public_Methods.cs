CxList paramDecl = All.FindByType(typeof(ParamDecl));
CxList allUnkRef = All.FindByType(typeof(UnknownReference)) + paramDecl;
CxList memberAccess = All.FindByType(typeof(MemberAccess)).GetTargetOfMembers().FindByType(typeof(UnknownReference));
CxList paramDeclMemberAccess = memberAccess + paramDecl;

CxList publicMethods = All.FindByFieldAttributes(Modifiers.Public).FindByType(typeof(MethodDecl));

CxList allIfStmt = All.FindByType(typeof(IfStmt));
CxList allTryCatchFinallyStmt = All.FindByType(typeof(TryCatchFinallyStmt));
CxList allMethodInvokeExpr = Find_Methods();

paramDecl -= paramDecl.FindByTypes(new String[] {"bool", "System.Boolean",
	"byte", "System.Byte", "sbyte", "System.SByte",
	"short", "System.Int16", "ushort", "System.UInt16",
	"int", "System.Int32", "uint", "System.UInt32",
	"long", "System.Int64", "ulong", "System.UInt64",
	"float", "System.Single",
	"double", "System.Double",
	"decimal", "System.Decimal",
	"char", "System.Char",
	"DateTime", "System.DateTime", "CancellationToken"});

CxList publicMethodsWithParams = paramDecl.GetAncOfType(typeof(MethodDecl)) * publicMethods;
CxList paramsInPublicMethods = paramDecl.GetByAncs(publicMethodsWithParams);
CxList nullInIfStmt = All.FindByType(typeof(NullLiteral)).GetByAncs(allIfStmt);

CxList checkedParamsInPublicMethods = paramsInPublicMethods;
CxList IsNull = All.FindByShortName("IsNull*");
CxList isNullParameters = All.GetParameters(IsNull);

// Caluculate CxLists that will be used inside the loop (performance optimization)
CxList params0 = paramDecl.GetByAncs(publicMethodsWithParams);
CxList methodParamReferences1 = paramDeclMemberAccess.FindAllReferences(params0);
CxList methodParamReferences2 = allUnkRef.FindAllReferences(params0);
CxList declInPublicMethods = paramDecl.GetByAncs(publicMethodsWithParams);
CxList ifStmtInPublicMethods = allIfStmt.GetByAncs(publicMethodsWithParams);

foreach(CxList method in publicMethodsWithParams)
{
	// Caluculate CxLists that will be used inside the inner loop (performance optimization)
	CxList paramsInMethod = declInPublicMethods.GetByAncs(method);
	CxList ifStmtInMethod = ifStmtInPublicMethods.GetByAncs(method);
	CxList paramsInMethodReferences = methodParamReferences1.FindAllReferences(paramsInMethod);
	CxList paramMemberAccessReferences = methodParamReferences2.FindAllReferences(paramsInMethod);
	foreach(CxList paramInMethod in paramsInMethod)
	{
		CxList paramMemberAccessInMethodReferences = paramsInMethodReferences.FindAllReferences(paramInMethod);
		CxList paramInMethodReferences = paramMemberAccessReferences.FindAllReferences(paramInMethod);

		if((paramMemberAccessInMethodReferences - paramInMethod - 
			paramMemberAccessInMethodReferences.GetByAncs(allMethodInvokeExpr) -
			paramMemberAccessInMethodReferences.GetByAncs(allTryCatchFinallyStmt)).Count == 0)
		{
			continue;
		}
		
		CxList paramInIfStmt = paramInMethodReferences.GetByAncs(ifStmtInMethod);
		
		CxList GetMethodIfThisParamIsChecked = paramInIfStmt.FindByFathers(paramInIfStmt.GetFathers() * nullInIfStmt.GetFathers()).GetAncOfType(typeof(MethodDecl));

		if(GetMethodIfThisParamIsChecked.Count == 0)
		{
			checkedParamsInPublicMethods -= paramInMethod;
		}
		
		if((paramInIfStmt * isNullParameters).Count > 0)
		{
			checkedParamsInPublicMethods.Add(paramInMethod);
		}
	}

}
//	When preprocessing razor templates, their contents are wrapped in a 
//	generated public method that shouldn't be a result.
CxList publicMethodsInRazorTemplates = paramsInPublicMethods.FindByFileName("*.cshtml");
paramsInPublicMethods -= publicMethodsInRazorTemplates;
	
result = paramsInPublicMethods - checkedParamsInPublicMethods;