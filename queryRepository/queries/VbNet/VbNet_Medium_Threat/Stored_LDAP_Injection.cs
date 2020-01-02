CxList inputs = Find_Read();
inputs.Add(Find_DB_Out());
CxList dirs = Find_LDAP_Output();
CxList sanitize = Find_LDAP_Sanitize();

result = dirs.InfluencedByAndNotSanitized(inputs, sanitize);