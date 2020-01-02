CxList sanitized = All.NewCxList();
sanitized.Add(Find_Integers());
sanitized.Add(Find_String_Sanitize());

CxList methods = Find_Methods();
CxList htmlSpecialChars = methods.FindByShortName("htmlspecialchars");
//blacklist for htmlspecialchars flags
List < string > htmlSpecialCharsBadFlag = new List<string>{"ENT_IGNORE"};
//remove htmlspecialchars that use flags from blacklist in second parameter
htmlSpecialChars -= htmlSpecialChars.FindByParameters(All.GetParameters(htmlSpecialChars, 1).FindByShortNames(htmlSpecialCharsBadFlag));
sanitized.Add(All.GetParameters(htmlSpecialChars, 0));

CxList filterVar = methods.FindByShortName("filter_var");
//whitelist of flags to be used in filter_var second parameter in order to sanitize the input
List <string> sanitizeFilters = new List<string>{"FILTER_SANITIZE_EMAIL", "FILTER_SANITIZE_MAGIC_QUOTES",
		"FILTER_SANITIZE_NUMBER_FLOAT", "FILTER_SANITIZE_NUMBER_INT",
		"FILTER_SANITIZE_SPECIAL_CHARS", "FILTER_SANITIZE_FULL_SPECIAL_CHARS",
		"FILTER_SANITIZE_STRING", "FILTER_SANITIZE_STRIPPED", "FILTER_SANITIZE_URL"};
CxList sanitizedFilterVar = filterVar.FindByParameters(All.GetParameters(filterVar, 1).FindByShortNames(sanitizeFilters));
sanitized.Add(All.GetParameters(sanitizedFilterVar, 0));

//this function will escape the following chars NUL (ASCII 0), \n, \r, \, ', ", and Control-Z
CxList mysqliRealEscapeString = methods.FindByShortName("mysqli_real_escape_string");
sanitized.Add(All.GetParameters(mysqliRealEscapeString, 1));

result = sanitized;