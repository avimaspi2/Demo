CxList allUnkRef = All.FindByType(typeof(UnknownReference)) + All.FindByType(typeof(ParamDecl));
CxList memberAccess = (All.FindByType(typeof(MemberAccess))).GetTargetOfMembers().FindByType(typeof(UnknownReference));
CxList paramDeclMemberAccess = memberAccess + All.FindByType(typeof(ParamDecl));

CxList publicMethods = All.FindByFieldAttributes(Modifiers.Public).FindByType(typeof(MethodDecl));

CxList paramDecl = All.FindByType(typeof(ParamDecl));
CxList allIfStmt = All.FindByType(typeof(IfStmt));
CxList allTryCatchFinallyStmt = All.FindByType(typeof(TryCatchFinallyStmt));
CxList allMethodInvokeExpr = Find_Methods();

paramDecl -= paramDecl.FindByType("bool");
paramDecl -= paramDecl.FindByType("byte");
paramDecl -= paramDecl.FindByType("sbyte");
paramDecl -= paramDecl.FindByType("short");
paramDecl -= paramDecl.FindByType("ushort");
paramDecl -= paramDecl.FindByType("int");
paramDecl -= paramDecl.FindByType("uint");
paramDecl -= paramDecl.FindByType("long");
paramDecl -= paramDecl.FindByType("ulong");
paramDecl -= paramDecl.FindByType("float");
paramDecl -= paramDecl.FindByType("double");
paramDecl -= paramDecl.FindByType("decimal");
paramDecl -= paramDecl.FindByType("char");
paramDecl -= paramDecl.FindByType("datetime");

CxList publicMethodsWithParams = paramDecl.GetAncOfType(typeof(MethodDecl)) * publicMethods;
CxList paramsInPublicMethods = paramDecl.GetByAncs(publicMethodsWithParams);
CxList nullInIfStmt = All.FindByName("null").GetByAncs(allIfStmt);

CxList checkedParamsInPublicMethods = paramsInPublicMethods;
CxList IsNull = All.FindByShortName("isnull*");

foreach(CxList method in publicMethodsWithParams)
{
	CxList paramsInMethod = paramDecl.GetByAncs(method);
	CxList ifStmtInMethod = allIfStmt.GetByAncs(method);

	foreach(CxList paramInMethod in paramsInMethod)
	{
		CxList paramMemberAccessInMethodReferences = paramDeclMemberAccess.FindAllReferences(paramInMethod);
		CxList paramInMethodReferences = allUnkRef.FindAllReferences(paramInMethod);

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

		if((paramInIfStmt * All.GetParameters(IsNull)).Count > 0)
		{
			checkedParamsInPublicMethods.Add(paramInMethod);
		}
	}
}

result = paramsInPublicMethods - checkedParamsInPublicMethods;