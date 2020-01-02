CxList evilStrings = Find_Evil_Strings_For_Replace();

// Find all regex commands
CxList regex = Find_Replace_Param();

// Find regex commands that are influenced by evil strings
CxList activeEvilRegexes = evilStrings.DataInfluencingOn(regex);

regex -= regex.DataInfluencedBy(Find_Inputs());

// Find relevant matches
CxList result1 = activeEvilRegexes.DataInfluencingOn(regex);

// Add static regexes (these do not influence their references, so needed in addition)
CxList staticFields = regex.FindByType(typeof(FieldDecl)).FindByFieldAttributes(Modifiers.Static);
CxList result2 = regex.GetByAncs(staticFields).DataInfluencedBy(evilStrings);

// Add the results
result.Add(result1);
result.Add(result2);