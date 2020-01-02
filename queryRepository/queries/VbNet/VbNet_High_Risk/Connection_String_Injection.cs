CxList con = Find_Connection_String();
CxList inputs = Find_Interactive_Inputs();

CxList sanitize = Find_General_Sanitize();
	
result = con.InfluencedByAndNotSanitized(inputs, sanitize);