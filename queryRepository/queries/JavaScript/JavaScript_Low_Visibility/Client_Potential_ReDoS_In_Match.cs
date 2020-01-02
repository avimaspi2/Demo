CxList evilStrings = Find_Evil_Strings();

// Find all regex commands
CxList regex = Find_Match();

// Find regex commands that are influenced by evil strings
CxList activeEvilRegexes = evilStrings.DataInfluencingOn(regex);

regex -= regex.DataInfluencedBy(Find_Inputs());

// Find relevant matches
CxList result1 = activeEvilRegexes.DataInfluencingOn(regex);

// Add static regexes (these do not influence their references, so needed in addition)
CxList staticFields = regex.FindByType(typeof(FieldDecl)).FindByFieldAttributes(Modifiers.Static);
CxList result2 = regex.GetByAncs(staticFields).DataInfluencedBy(evilStrings);

// If only one type is found, no need to combine CxLists, because combining might lose the path
if (result1.Count > 0 && result2.Count > 0)
{
	result = result1;
	result.Add(result2);
}
else if (result1.Count > 0)
{
	result = result1;
}
else
{
	result = result2;
}