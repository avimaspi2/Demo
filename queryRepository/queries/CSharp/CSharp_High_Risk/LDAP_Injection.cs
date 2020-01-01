CxList ldap = Find_LDAP_Output();

CxList inputs = Find_Interactive_Inputs();

CxList sanitize = Find_LDAP_Sanitize();

result = ldap.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);