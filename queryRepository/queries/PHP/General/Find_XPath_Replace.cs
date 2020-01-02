CxList methods = Find_Methods();
CxList replace = methods.FindByShortNames(new List<string>
	{"str_replace", "stri_replace", "preg_filter"});
//	methods.FindByShortName("str_replace") + 
//	methods.FindByShortName("stri_replace") +
//	methods.FindByShortName("preg_filter");

CxList parameters = All.GetParameters(replace, 0).FindByType(typeof(StringLiteral));

parameters = parameters.FindByShortNames(new List<string> {"'", "\\'", "\\\\'","/'","//'","(",")", " or "});
//	parameters.FindByShortName("'") + 
//	parameters.FindByShortName("\\'") + 
//	parameters.FindByShortName("\\\\'") +
//	parameters.FindByShortName("/'") + 
//	parameters.FindByShortName("//'") + 
//	parameters.FindByShortName("(") +
//	parameters.FindByShortName(")") + 
//	parameters.FindByShortName(" or ");

result = methods.FindByShortNames(new List<string> 
		{"preg_quote","preg_replace_callback","preg_replace","ereg_replace","eregi_replace"}) +
//	methods.FindByShortName("preg_quote") +
//	methods.FindByShortName("preg_replace_callback") +
//	methods.FindByShortName("preg_replace") +
//	methods.FindByShortName("ereg_replace") +
//	methods.FindByShortName("eregi_replace") +
	replace.FindByParameters(parameters);