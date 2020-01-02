CxList evilStrings = Find_Evil_Strings();

// Find all regex commands, not including groovy special regex syntax
CxList regex = All.FindByMemberAccess("Pattern.compile");

// Find regex commands that are influenced by evil strings
CxList activeEvilRegexes = evilStrings.DataInfluencingOn(regex);

// Find all matches/splits of regexes
CxList match = All.FindByMemberAccess("Matcher.matches");
match = match.DataInfluencedBy(Find_Inputs());
CxList split = All.FindByMemberAccess("Pattern.split");
split = split.DataInfluencedBy(Find_Inputs());

// Find relevant matches
result = activeEvilRegexes.DataInfluencingOn(match + split);