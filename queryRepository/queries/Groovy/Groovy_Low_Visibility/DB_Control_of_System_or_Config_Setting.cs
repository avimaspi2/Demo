CxList methods = Find_Methods();
CxList setters = 
	methods.FindByShortName("setProperty") + 
	methods.FindByShortName("setProperties") +
	methods.FindByShortName("setCatalog");

CxList sanitize = Find_General_Sanitize();
CxList inputs = Find_DB_Out() + Find_Read_NonDB();

result = inputs.InfluencingOnAndNotSanitized(setters, sanitize);