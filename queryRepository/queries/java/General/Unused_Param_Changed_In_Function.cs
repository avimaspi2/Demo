// Custom attributes
CxList customAttributes = Find_UnknownReference().GetByAncs(Find_CustomAttribute());

// Dead Code (and JSP code, that should be ignored as well)
CxList jsp = Find_Jsp_Code();
jsp = jsp.GetByAncs(jsp.FindByShortName("Checkmarx_Class_Init"));
CxList deadCode = Find_Dead_Code_Contents();
deadCode.Add(jsp);

CxList paramDecl = Find_ParamDeclaration();
paramDecl -= deadCode;

CxList vars = Find_Constant_And_Artificially_Variables();

// Have a list of variable-and-param decl, for the first type of unused variables
CxList varsAndParams = All.NewCxList();
varsAndParams.Add(vars);
varsAndParams.Add(paramDecl);
// All references of vars (and params)
CxList varsReferences = All.FindAllReferences(varsAndParams) - customAttributes;

// Find only paramDecl that are set to a value (for optimization purposes, no need to loop on any others
CxList setParamDecl = varsReferences.FindAllReferences(paramDecl).FindByAssignmentSide(CxList.AssignmentSide.Left);
setParamDecl = paramDecl.FindDefinition(setParamDecl);

CxList methodsContent = All.FindAllReferences(setParamDecl).GetByAncs(paramDecl.GetAncOfType(typeof(MethodDecl)));
methodsContent -= deadCode;
CxList changedParams = All.NewCxList();
// Look at every parameter and check if it is set a value and then used

foreach (CxList oneParamDecl in setParamDecl)
{
	CxList method = oneParamDecl.GetAncOfType(typeof(MethodDecl));
	CxList usedParams = methodsContent.GetByAncs(method) - oneParamDecl;
	usedParams = usedParams.FindAllReferences(oneParamDecl);
	CxList paramAssign = usedParams.GetAncOfType(typeof(AssignExpr));
	foreach (CxList pAssign in paramAssign)
	{
		CxList usedParamUnderAssign = usedParams.GetByAncs(pAssign);
		if (usedParamUnderAssign.Count > 1)
		{
			usedParams -= usedParamUnderAssign;
		}
	}
	CxList usedParamsInfluenced = usedParams.DataInfluencedBy(oneParamDecl);
	CxList usedParamsNotInfluenced = usedParams - usedParamsInfluenced;
	changedParams.Add(usedParamsNotInfluenced.DataInfluencingOn(usedParamsNotInfluenced));
}

changedParams -= changedParams.GetMembersOfTarget().GetTargetOfMembers();
changedParams -= changedParams.DataInfluencingOn(changedParams);

result = changedParams;