/* Citrus Webx Inputs */
if (Find_Citrus_Framework().Count > 0)
{
	CxList unkRefs = Find_UnknownReference();
	CxList calls = Find_Methods(); 
	calls.Add(Find_MemberAccess());
	
	// ParameterParser 
	CxList citrusInputs = unkRefs.FindByType("ParameterParser").GetMembersOfTarget().FindByShortName("get*");

	// Get all invocations of TurbineRunData.getParameters().xxxx that return the parameters from Citrus framework
	citrusInputs.Add(calls.FindByMemberAccess("TurbineRunData.getParameters").GetMembersOfTarget().FindByShortName("get*"));

	// Parameters declared with the @Param annotation
	List<string> inputAnnotations = new List<string> {"Param", "Params", "ContextValue", "FormGroup", "FormGroups"};
	CxList annotations = Find_CustomAttribute().FindByShortNames(inputAnnotations);
	CxList annoDecls = annotations.GetAncOfType(typeof(ParamDecl));
	citrusInputs.Add(annoDecls);

	citrusInputs.Add(calls.FindByExactMemberAccess("Context.get"));

	result = citrusInputs;
}