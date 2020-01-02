CxList inputs = Find_Read();
inputs.Add(Find_DB_Out());

CxList sanitize = Find_LDAP_Sanitize(); 
CxList outputs = Find_LDAP_Inputs();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);