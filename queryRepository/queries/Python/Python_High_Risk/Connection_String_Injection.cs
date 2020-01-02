CxList con = Find_DB_Connections();
CxList inputs = Find_Interactive_Inputs();

CxList sanitize = Find_Sanitize();
sanitize.Add(Find_Integers());

result = con.InfluencedByAndNotSanitized(inputs, sanitize);