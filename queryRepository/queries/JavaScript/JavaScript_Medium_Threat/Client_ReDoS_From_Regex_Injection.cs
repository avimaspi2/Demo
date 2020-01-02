CxList inputs = Find_Inputs();
CxList evilStrings = inputs.Clone();

// Find all relevant regex commands
CxList match = Find_Match();
match.Add(Find_Replace());
CxList matchParameters = All.GetByAncs(All.GetParameters(match));

// Find their Regexes
CxList regex = matchParameters.GetByAncs(matchParameters.GetParameters(match, 0));

// Find only matches that are influenced by an input, then use only their regexes
match = match.InfluencedByAndNotSanitized(inputs, matchParameters);
regex = matchParameters.GetByAncs(matchParameters.GetParameters(match, 0));

// Find regex commands that are influenced by evil strings
CxList activeEvilRegexes = evilStrings * regex;
activeEvilRegexes.Add(evilStrings.DataInfluencingOn(regex));

// Sanitization
CxList sanitize = All.NewCxList() - Find_Replace_Param();
result = regex.InfluencedByAndNotSanitized(activeEvilRegexes, sanitize);


//now handle the RegExp
//RegExp has the following methods: exec, and test and is activated like this: RegExpObject.exec(string)
CxList methods = Find_Methods();
CxList execOrTest = methods.FindByShortNames(new List<string>{"exec",  "test"});

//now the conditions:
//get the parameter of the exec and test:

CxList unknownReference = Find_UnknownReference();
sanitize = Sanitize();
//in case they are influenced by inputs
CxList paramInfluencedByInputs = unknownReference.GetByAncs(All.GetParameters(execOrTest)).InfluencedByAndNotSanitized(evilStrings, sanitize);

//now get to its regex and see if it's influenced by evil strings
regex = execOrTest.FindByParameters(paramInfluencedByInputs).GetTargetOfMembers();
result.Add(regex * evilStrings);
result.Add(regex.DataInfluencedBy(evilStrings));