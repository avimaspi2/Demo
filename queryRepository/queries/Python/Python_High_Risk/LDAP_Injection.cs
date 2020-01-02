CxList inputs = Find_Interactive_Inputs();
CxList outputs = Find_LDAP_Inputs();
CxList sanitize = Find_LDAP_Sanitize(); 

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);