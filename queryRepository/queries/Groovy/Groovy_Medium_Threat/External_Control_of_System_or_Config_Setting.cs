CxList methods = Find_Methods();
CxList setters = 
	methods.FindByShortName("setProperty") + 
	methods.FindByShortName("setProperties") +
	methods.FindByShortName("setCatalog");

CxList sanitize = Find_General_Sanitize();
CxList inputs = Find_Interactive_Inputs();

result = inputs.InfluencingOnAndNotSanitized(setters, sanitize);