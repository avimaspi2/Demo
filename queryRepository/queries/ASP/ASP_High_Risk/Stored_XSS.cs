CxList db = Find_DB_Out();
CxList outputs = Find_XSS_Outputs();
CxList sanitize = Find_XSS_Sanitize() - Find_LDAP();

result = All.FindXSS(db + Find_IO(), outputs, sanitize);