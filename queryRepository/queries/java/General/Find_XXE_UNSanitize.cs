CxList trueFalse = All.NewCxList();
trueFalse.Add(All.FindByShortName("true", false));
trueFalse.Add(All.FindByShortName("false", false));

CxList bools = trueFalse.FindByType(typeof(Param));

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


CxList UNsanitTrueFalse = All.NewCxList();
UNsanitTrueFalse.Add(UNsanit_true);
UNsanitTrueFalse.Add(UNsanit_false);

result = sanitizers * UNsanitTrueFalse;