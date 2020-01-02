CxList truesFalses = All.FindByShortName("true", false);
truesFalses.Add(All.FindByShortName("false", false));

CxList bools = truesFalses.FindByType(typeof(Param));

CxList sanitizers = Find_XXE_Sanitize_Methods();

CxList xxeFeatTrue = Find_XXE_Features_TRUE();
CxList xxeFeatFalse = Find_XXE_Features_FALSE();

CxList sanitTrue = sanitizers.InfluencedBy(xxeFeatTrue)
	.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

sanitTrue = sanitTrue * bools.FindByShortName("true", false).GetAncOfType(typeof(MethodInvokeExpr));

CxList sanitFalse = sanitizers.InfluencedBy(xxeFeatFalse)
	.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

sanitFalse = sanitFalse * bools.FindByShortName("false", false).GetAncOfType(typeof(MethodInvokeExpr));

CxList sanitTrueFalse = All.NewCxList();
sanitTrueFalse.Add(sanitTrue);
sanitTrueFalse.Add(sanitFalse);

result = sanitizers * sanitTrueFalse;

/*avoid  factories with external entities disabled*/
result.Add(All.FindAllReferences(result.GetTargetOfMembers()).GetMembersOfTarget().FindByShortName("createXMLEventReader"));