CxList methods = Find_Methods();
CxList replace = 
	methods.FindByShortName("str_replace") + 
	methods.FindByShortName("stri_replace") ;

CxList parameters = All.GetParameters(replace, 0).FindByType(typeof(StringLiteral));

parameters = parameters.FindByShortNames(new List<string>{"(", ")", "=","&","|","!"});

result = methods.FindByShortNames(new List<string>
	{"preg_quote", "preg_filter", "preg_replace_callback","preg_replace","ereg_replace","eregi_replace","ldap_escape"})+
	replace.FindByParameters(parameters);