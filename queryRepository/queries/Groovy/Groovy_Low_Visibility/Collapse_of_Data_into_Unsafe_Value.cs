CxList inputs = Find_Interactive_Inputs();
CxList outputs = Find_XSS_Outputs();
CxList methods = Find_Methods();

// Find all Replace
CxList replace = methods.FindByShortName("replace*");

// Look at replace that is affected by inputs, and affecting outputs (potential XSS),
// but does not pass through a sanitizer
CxList sanitize = Find_XSS_Sanitize() + Find_DB_In();
CxList relevantReplace = replace
	.InfluencedByAndNotSanitized(inputs, sanitize)
	.InfluencingOnAndNotSanitized(outputs, sanitize);

// remove replaces in loop statement (which might be a good enough sanitization after all)
CxList loops = relevantReplace.GetAncOfType(typeof(IterationStmt));
CxList relevantReplaceInLoop = relevantReplace.GetByAncs(loops);

result = relevantReplace - relevantReplaceInLoop;