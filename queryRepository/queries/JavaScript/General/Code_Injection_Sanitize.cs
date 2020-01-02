// A general query for finding all JS-CodeInjection sanitizers
CxList generalSanitize = Sanitize();
result.Add(generalSanitize);
result.Add(Find_SAPUI5_Sanitize().FindByShortNames(new List<string> {"escapeJS", "encodeJS"}));