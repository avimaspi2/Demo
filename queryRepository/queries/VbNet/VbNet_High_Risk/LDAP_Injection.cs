CxList inputs = Find_Interactive_Inputs();
CxList dirs = Find_LDAP_Output();
CxList sanitize = Find_LDAP_Sanitize();

result = dirs.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);