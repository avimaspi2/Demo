CxList methods = Find_Methods();

// All the relevant (basic) methods
CxList scanMethods = 
	methods.FindByShortName("sscanf") + 
	methods.FindByShortName("swscanf") +
	methods.FindByShortName("sscanf_s") +
	methods.FindByShortName("swscanf_s") +
	methods.FindByShortName("sprintf") +
	methods.FindByShortName("swprintf");

CxList strings = Find_Strings();

strings = strings.FindByShortName("*%s*");

CxList secondParam = All.GetParameters(scanMethods, 1);

result = secondParam.DataInfluencedBy(strings) + secondParam * strings;