CxList db = Find_DB_Out();
CxList read = Find_Read();
CxList outputs = Find_Interactive_Outputs();
CxList sanitize = Find_XSS_Sanitize();

result = All.FindXSS(db + read, outputs, sanitize);