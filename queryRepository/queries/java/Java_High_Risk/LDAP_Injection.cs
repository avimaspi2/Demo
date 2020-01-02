CxList inputs = Find_Interactive_Inputs();
CxList sanitize = Find_LDAP_Sanitize(); 
CxList outputs = Find_LDAP_Outputs();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);