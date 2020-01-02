CxList ldap = Find_LDAP_Output();

CxList inputs = Find_Read() + Find_DB_Out();

CxList sanitize = Find_LDAP_Sanitize();

result = ldap.InfluencedByAndNotSanitized(inputs, sanitize);