CxList inputs = Find_Inputs();
CxList evilStrings = Find_Evil_Strings_For_Replace();

// Find all regex commands
CxList replace = Find_Replace();
CxList replaceParameters = All.GetByAncs(All.GetParameters(replace));

// Find their Regexes
CxList regex = replaceParameters.GetByAncs(replaceParameters.GetParameters(replace, 0));

// Find only matches that are influenced by an input, then use only their regexes
replace = replace.InfluencedByAndNotSanitized(inputs, replaceParameters);
regex = replaceParameters.GetByAncs(replaceParameters.GetParameters(replace, 0));

// Find regex commands that are influenced by evil strings
CxList activeEvilRegexes = evilStrings * regex;
activeEvilRegexes.Add(evilStrings.DataInfluencingOn(regex));

// Sanitization
CxList sanitize = Sanitize() - Find_Replace_Param();

// Find relevant matches
result = replace.InfluencedByAndNotSanitized(activeEvilRegexes, sanitize);
result -= result.DataInfluencedBy(result);