CxList inputs = Find_Interactive_Inputs();
CxList decode = All.FindByName("*Server.HtmlDecode", false).DataInfluencedBy(inputs);

CxList sanitize = Find_Sanitize();
CxList db = Find_SQL_DB_In();

result = db.InfluencedByAndNotSanitized(decode, sanitize);