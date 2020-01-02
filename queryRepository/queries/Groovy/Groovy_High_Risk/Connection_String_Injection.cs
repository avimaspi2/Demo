CxList con = All.FindByName("*getConnection");

CxList inputs = Find_Interactive_Inputs();
CxList sanitize = Find_General_Sanitize() + Find_Integers();

result = con.InfluencedByAndNotSanitized(inputs, sanitize);