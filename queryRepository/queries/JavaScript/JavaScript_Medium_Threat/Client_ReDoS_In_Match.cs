// Query Client_ReDoS_In_Match
// //////////////////////////-
// The purpose of the query to find ReDos vulnerability in Match and Split commands

CxList inputs = Find_Inputs();
CxList evilStrings = Find_Evil_Strings();

// Find all regex commands
CxList match = Find_Match();
CxList matchParameters = All.GetByAncs(All.GetParameters(match)); // list of match and split params 

CxList matchAndSplitFirstParam = matchParameters.GetParameters(match, 0);


// Find only matches that are influenced by an input, then use only their regexes
match = match.InfluencedByAndNotSanitized(inputs, matchParameters);

// matchParameters will include the list match and split parameters that affected by input
matchParameters = All.GetByAncs(All.GetParameters(match, 0));

// Find their Regexes
CxList regex = matchParameters.GetByAncs(matchAndSplitFirstParam);
// //// At this time the "regex" variable includes the list of firsts parameters

// Find regex commands that are influenced by evil strings
CxList activeEvilRegexes = (evilStrings * regex);
activeEvilRegexes.Add(evilStrings.DataInfluencingOn(regex));

//optimization
CxList methods = Find_Methods();

CxList splitAndMatchMethods = methods.FindByMemberAccess("*.split", false);
splitAndMatchMethods.Add(methods.FindByMemberAccess("*.match", false));

CxList splitAndMatchMethodsParams = All.GetParameters(splitAndMatchMethods, 0);


// In  query approach the sync is match and split PARAMETERS, but functions it self will be sanitizers
// //////////////////////////////////////////////////////////////////////////////////////////////////-

// Sanitization
CxList sanitize = methods.FindByMemberAccess("*.match", false);
sanitize.Add(methods.FindByMemberAccess("*.split", false));
sanitize.Add(Sanitize());
sanitize.Add(Find_Replace());
sanitize -= splitAndMatchMethodsParams;

// Find relevant matches
CxList flows = matchParameters.InfluencedByAndNotSanitized(activeEvilRegexes, sanitize);
CxList evilParams = activeEvilRegexes * matchParameters;

result = flows;

//In order to build the flow from match parameter to match methods the loop below do this action
foreach (CxList r in evilParams)
{
	result.Add(r.Concatenate(methods.FindByParameters(r)));
}