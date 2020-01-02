CxList evilStrings = Find_Evil_Strings();
CxList inputs = Find_Inputs();

// Find all regex commands
CxList regex = All.FindByMemberAccess("Pattern.compile");

// Find regex commands that are influenced by evil strings
CxList activeEvilRegexes = evilStrings.DataInfluencingOn(regex);

// Find all matches/splits of regexes
CxList match = All.FindByMemberAccess("Matcher.matches");
match = match.DataInfluencedBy(inputs);
CxList split = All.FindByMemberAccess("Pattern.split");
split = split.DataInfluencedBy(inputs);

CxList matchSplit = All.NewCxList();
matchSplit.Add(match);
matchSplit.Add(split);

// Find relevant matches
result = activeEvilRegexes.DataInfluencingOn(matchSplit);