List < String > sanitizeList = new List<String> {"validateLongRange", 
		"validateDoubleRange", 
		"validateRegex",
		"convertDateTime",
		"convertNumber"};
CxList jsfInputs = All.FindByShortName("CxJsfInput");
CxList sanitize = All.FindByShortNames(sanitizeList);
result = jsfInputs.GetByAncs(sanitize);