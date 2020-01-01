CxList con = Find_Connection_String();
CxList inputs = Find_Interactive_Inputs();
CxList sanitize = Find_Sanitize(); // Might be too broad since it includes sql-specific sanitizers, but good enough
	
result = con.InfluencedByAndNotSanitized(inputs, sanitize);