CxList con = Find_Connection();

CxList inputs = Find_Interactive_Inputs();
CxList sanitize = Find_General_Sanitize();
sanitize.Add(Find_Integers());

result = con.InfluencedByAndNotSanitized(inputs, sanitize);