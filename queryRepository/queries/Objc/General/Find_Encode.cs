CxList methods = Find_Methods();

CxList encode = methods.FindByShortName("*encode*", false) - 
	methods.FindByShortName("*unencode*", false);

encode.Add(methods.FindByShortNames(
	new List<string>{
		"*htmlentities*", "*htmlspecialchars*", "*filter_var*", "*strip_tags*", 
		"fix_quotes", "esc*", "*esc", "*escape*", "sanitize*"
		},
	false
	));
encode.Add(All.GetByAncs(methods.FindByShortName("h")));

result = encode;