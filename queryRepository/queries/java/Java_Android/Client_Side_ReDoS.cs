// Query Client_Side_DoS
// ---------------------
CxList evilStringsForReplace = Find_Evil_Strings_For_Replace();
CxList match = Find_Match();
CxList inputs = Find_Inputs();

CxList regexInFirstParam = Find_Replace_Regex_In_First_Param();
CxList regexInSecondParam = Find_Replace_Regex_In_Second_Param();


//Find In Pattern
CxList evilStrings = Find_Evil_Strings();

// Find In Match
result = Find_ReDoS(evilStrings, match, 0, false);

// Find In Replace
result.Add(Find_ReDoS(evilStringsForReplace, regexInFirstParam, 0, false));
result.Add(Find_ReDoS(evilStringsForReplace, regexInSecondParam, 1, false));


// Find all regex commands
CxList regex = All.FindByMemberAccess("Pattern.compile");
// Find regex commands that are influenced by evil strings
CxList activeEvilRegexes = evilStrings.DataInfluencingOn(regex);
// Find all matches/splits of regexes
CxList matches = All.FindByMemberAccess("Matcher.matches");
matches = matches.DataInfluencedBy(inputs);
CxList split = All.FindByMemberAccess("Pattern.split");
split = split.DataInfluencedBy(inputs);

// Find relevant matches
CxList matchesSplit = All.NewCxList();
matchesSplit.Add(matches);
matchesSplit.Add(split);

result.Add(activeEvilRegexes.DataInfluencingOn(matchesSplit));

// ReDoS from Regex Injection
CxList methods = Find_Methods();

CxList inputsMember =  All.NewCxList();
inputsMember.Add(inputs);
inputsMember.Add(methods.FindByMemberAccess("SmsMessage.get*"));
inputsMember.Add(methods.FindByMemberAccess("Folder.get*"));

CxList matchFirstParam =  All.NewCxList();
matchFirstParam.Add(match);
matchFirstParam.Add(regexInFirstParam);
matchFirstParam.Add(All.FindByName("Pattern.compile"));

result.Add(Find_ReDoS(inputsMember, matchFirstParam, 0, false));
result.Add(Find_ReDoS(regexInSecondParam, 1, false));