CxList db = Find_DB();
CxList outputs = Find_Interactive_Outputs();

CxList sanitize = Find_XSS_Sanitize();
result = All.FindXSS(db + Find_IO(), outputs, sanitize);