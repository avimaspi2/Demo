CxList inputs = Find_Inputs();
CxList styleAttributes = React_Find_PropertyKeys().FindByShortName("style");
CxList baseSanitize = basic_Sanitize();
result = inputs.InfluencingOnAndNotSanitized(styleAttributes, baseSanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);