// Find the methods that can return escaped strings
CxList methods = Find_Methods();
CxList replace = 
	methods.FindByShortNames(new List<string>
	{"str_replace", "stri_replace", "preg_filter", "preg_replace_callback",
		"preg_replace","ereg_replace","eregi_replace","stri_replace"});

//Get the first parameter from the methods
CxList parameters = All.GetParameters(replace, 0);
CxList str_parameters = parameters.FindByType(typeof(StringLiteral));
//The first parameter can also be an array of strings to replace
CxList arrays = parameters.FindByShortName("array").FindByType(typeof(MethodInvokeExpr));
str_parameters.Add(All.GetParameters(arrays).FindByType(typeof(StringLiteral)));
str_parameters = str_parameters.FindByShortNames(new List<string> {"'","\\'","\\\\'","/'","//'","<",">","\"","\\\"","/\"","//\""});

//"preg_quote" method already return escaped strings 
result = methods.FindByShortName("preg_quote");
//Get the 'replace' methods that have arrays in the 1st parameter
result.Add(replace.FindByParameters(arrays.FindByParameters(str_parameters)));
result.Add(replace.FindByParameters(str_parameters));