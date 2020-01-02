CxList bools = (All.FindByShortName("true", false) + All.FindByShortName("false", false)).FindByType(typeof(Param));

CxList sanitizers = Find_XXE_Sanitize_Methods();

CxList xxe_feat_true = Find_XXE_Features_TRUE();
CxList xxe_feat_false = Find_XXE_Features_FALSE();

CxList sanit_true = sanitizers.InfluencedBy(xxe_feat_true)
	.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
sanit_true = sanit_true * bools.FindByShortName("true", false).GetAncOfType(typeof(MethodInvokeExpr));

CxList sanit_false = sanitizers.InfluencedBy(xxe_feat_false)
	.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
sanit_false = sanit_false * bools.FindByShortName("false", false).GetAncOfType(typeof(MethodInvokeExpr));

result = sanitizers * (sanit_true + sanit_false);