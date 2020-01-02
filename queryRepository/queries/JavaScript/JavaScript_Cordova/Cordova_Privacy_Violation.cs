CxList outputs = Find_Outputs();
outputs.Add(PhoneGap_Find_Outputs());

CxList sanitizers = Find_Encrypt();
CxList sensitiveInfo = PhoneGap_Find_Personal_Info();
CxList vulnerablePaths = outputs.InfluencedByAndNotSanitized(sensitiveInfo, sanitizers);

result.Add(vulnerablePaths);
result.Add(Concatenate_Callback_Input_With_Its_Invocation(vulnerablePaths));
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);