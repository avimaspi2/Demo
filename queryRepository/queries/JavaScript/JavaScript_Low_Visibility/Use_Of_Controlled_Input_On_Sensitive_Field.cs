//If var disableInputAttributeSyncing = true; exists in the code, then it is sanitized for the whole project
CxList allDecls = Find_Declarators();
CxList allRefs = Find_UnknownReference();
CxList relevantDecl = allDecls.FindByShortName("disableInputAttributeSyncing");
CxList allRelevantDeclRefs = allRefs.FindAllReferences(relevantDecl);
CxList sanitized = allRelevantDeclRefs.GetAssigner().FindByType(typeof(BooleanLiteral)).FindByShortName("true");

if(sanitized.Count == 0) {
	CxList reactAll = React_Find_All();
	CxList methods = Find_Methods() * reactAll;
	CxList declarators = allDecls * reactAll;
	CxList memberAccesses = Find_MemberAccesses() * reactAll;

	CxList personalInfo = Find_Personal_Info();
	CxList reactInputs = React_Find_Input_Tags();

	CxList reactInputsMethods = reactInputs.GetAncOfType(typeof(MethodInvokeExpr));
	CxList cElement = methods.FindByMemberAccess("React.createElement");
	CxList methodsWithInput = reactInputsMethods * cElement;

	CxList methodsWithPersonalInfo = personalInfo.GetAncOfType(typeof(MethodInvokeExpr));
	methodsWithPersonalInfo = methodsWithPersonalInfo * methodsWithInput;

	CxList valueInMethod = (declarators.GetByAncs(methodsWithPersonalInfo)).FindByShortName("value");
	CxList values = (memberAccesses.GetByAncs(methodsWithPersonalInfo)).FindByShortName("value");
	CxList vulnerableValues = values.GetByAncs(valueInMethod);

	result = vulnerableValues;
}