CxList allUnkRef = All.FindByType(typeof(UnknownReference));
allUnkRef.Add(All.FindByType(typeof(ParamDecl)));
CxList memberAccess = (All.FindByType(typeof(MemberAccess))).GetTargetOfMembers().FindByType(typeof(UnknownReference));
CxList paramDeclMemberAccess = All.FindByType(typeof(ParamDecl));
paramDeclMemberAccess.Add(memberAccess);

CxList publicMethods = All.FindByFieldAttributes(Modifiers.Public).FindByType(typeof(MethodDecl));

CxList paramDecl = All.FindByType(typeof(ParamDecl));
CxList allIfStmt = All.FindByType(typeof(IfStmt));
CxList allTryCatchFinallyStmt = All.FindByType(typeof(TryCatchFinallyStmt));
CxList allMethodInvokeExpr = Find_Methods();

string[] types = {"Boolean", "byte", "sbyte", "short",  "ushort", "integer", "uinteger",  "long", "ulong", "float",  
	"double", "decimal", "char", "datetime"}; 
CxList removeTypes = paramDecl.FindByTypes(types, false);
paramDecl -= removeTypes;

CxList publicMethodsWithParams = paramDecl.GetAncOfType(typeof(MethodDecl)) * publicMethods;
CxList paramsInPublicMethods = paramDecl.GetByAncs(publicMethodsWithParams);
CxList nullInIfStmt = All.FindByName("null").GetByAncs(allIfStmt);

CxList checkedParamsInPublicMethods = paramsInPublicMethods;
CxList IsNull = All.FindByShortName("IsNull"); 
CxList nullChecked = IsNull.GetTargetOfMembers() + All.GetParameters(IsNull);

CxList NullableRefs = memberAccess.FindByType("System.Nullable", false);
CxList HasValue = NullableRefs.GetMembersOfTarget().FindByShortName("HasValue", false);
nullChecked.Add(HasValue.GetTargetOfMembers());

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

		if((paramInIfStmt * nullChecked).Count > 0)
		{
			checkedParamsInPublicMethods.Add(paramInMethod);
		}
	}
}

result = paramsInPublicMethods - checkedParamsInPublicMethods;