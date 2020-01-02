CxList inputs = Find_Inputs();
CxList outputs = Find_DB_In();
CxList methods = Find_Methods();

// Find all inappropriate sanitizers
CxList inapprSanitizers = methods.FindByShortNames(new List<String>(){ "mysql_escape_string", "mysql_real_escape_string" });

// Look at inapprSanitizers that is affected by inputs, and affecting outputs (potential XSS),
// but does not pass through a sanitizer
CxList sanitize = Find_SQL_Injection_Sanitize() - inapprSanitizers;

CxList relevantSanitizers = inapprSanitizers
	.InfluencedByAndNotSanitized(inputs, sanitize)
	.InfluencingOnAndNotSanitized(outputs, sanitize);

// remove inapprSanitizerss in loop statement (which might be a good enough sanitization after all)
CxList loops = relevantSanitizers.GetAncOfType(typeof(IterationStmt));
CxList relevantSanitizersInLoop = relevantSanitizers.GetByAncs(loops);

result = relevantSanitizers - relevantSanitizersInLoop;