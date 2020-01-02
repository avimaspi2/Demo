CxList methods = Find_Methods();
CxList sanitize = Find_XSS_Sanitize()
	- Find_XSS_Replace() 
	- Find_HTML_Encode(); // We know Find_HTML_Encode is too inclusive, but it's fine for this case

CxList replace = methods.FindByShortNames(new List<String>()
	{ "str_replace", "stri_replace", "preg_filter", "preg_replace_callback", 
		"preg_replace", "ereg_replace", "eregi_replace", "stri_replace" });

CxList parameters = All.GetParameters(replace, 0).FindByType(typeof(StringLiteral));
parameters = parameters.FindByShortNames(new List<String>(){"\n", "\r", "%0a", "%0d" });

sanitize.Add(replace.FindByParameters(parameters));

CxList inputs = Find_Interactive_Inputs();

CxList outputs = methods.FindByShortNames(new List<String>(){ "setrawcookie", 
		//These methods have built-in sanitization since version 4.4.2 and 5.1.2. 
		"setcookie", "header" });

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);