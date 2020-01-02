CxList bools = (All.FindByShortName("true", false) + All.FindByShortName("false", false)).FindByType(typeof(Param));

CxList sanitizers = Find_XXE_Sanitize_Methods();

CxList xxe_feat_true = Find_XXE_Features_TRUE();
CxList xxe_feat_false = Find_XXE_Features_FALSE();

//unsanitizers (those that should be true(resp. false) and are set false(resp. true) )
CxList UNsanit_true = sanitizers.InfluencedBy(xxe_feat_true)
	.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
UNsanit_true = UNsanit_true * bools.FindByShortName("false", false).GetAncOfType(typeof(MethodInvokeExpr));

CxList UNsanit_false = sanitizers.InfluencedBy(xxe_feat_false)
	.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
UNsanit_false = UNsanit_false * bools.FindByShortName("true", false).GetAncOfType(typeof(MethodInvokeExpr));


result = sanitizers * (UNsanit_true + UNsanit_false);