CxList db = Find_DB_Out() + Find_Read();
CxList outputs = Find_Interactive_Outputs(); ///TODO: check if sanitized when running cgi

CxList sanitize = Find_XSS_Sanitize();
result = db.InfluencingOnAndNotSanitized(outputs, sanitize);

result -= result.DataInfluencingOn(result);