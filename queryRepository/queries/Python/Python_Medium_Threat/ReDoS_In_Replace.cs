CxList replace = Find_Replace();
CxList evilStrings = Find_Evil_Strings();
CxList inputs = Find_Inputs();

CxList inputs_vars = inputs.GetAncOfType(typeof(AssignExpr)); // Assign or declare
inputs_vars.Add(inputs.GetAncOfType(typeof(Declarator)));

CxList inputsobjs = All.GetByAncs(inputs_vars).FindByAssignmentSide(CxList.AssignmentSide.Left); // assigned to
inputsobjs = All.FindAllReferences(inputsobjs);

// Find all regex patterns
CxList underReplace = All.GetByAncs(replace);
CxList regexPatternsAll = underReplace.GetByAncs(underReplace.GetParameters(replace, 0));

CxList regexPatterns = regexPatternsAll.FindByType(typeof(UnknownReference));
regexPatterns.Add(regexPatternsAll.FindByType(typeof(StringLiteral)));
regexPatterns.Add(regexPatternsAll.FindByType(typeof(MemberAccess)));

// Sanitization
CxList sanitize = Find_Integers();

// Find regex commands that are influenced by evil strings
CxList activeEvilRegexes = (evilStrings * regexPatterns);
activeEvilRegexes.Add(evilStrings.InfluencingOnAndNotSanitized(regexPatterns, sanitize));
activeEvilRegexes.Add(inputsobjs);

// Leave only relevant matches (strings influenced by input)
replace = replace.FindByParameters(activeEvilRegexes);

// Find relevant matches
result = replace.InfluencedByAndNotSanitized(activeEvilRegexes, sanitize)
	.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);