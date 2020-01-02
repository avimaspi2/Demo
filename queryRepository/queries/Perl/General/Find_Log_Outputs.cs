CxList methods = Find_Methods();
CxList log4perl = All.FindByName("Log::Log4perl");

CxList debugMethods = 
	methods.FindByShortName("debug") + 
	methods.FindByShortName("info") + 
	methods.FindByShortName("warn") + 
	methods.FindByShortName("error") + 
	methods.FindByShortName("fatal");

debugMethods *= debugMethods.DataInfluencedBy(All.FindByName("Log::Log4perl"));

result = debugMethods;