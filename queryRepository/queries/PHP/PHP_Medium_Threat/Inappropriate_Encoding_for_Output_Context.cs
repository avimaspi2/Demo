CxList inputs = Find_Inputs() + Find_DB();
CxList outputs = Find_Interactive_Outputs();
CxList methods = Find_Methods();

// Find all inappropriate sanitizers
CxList inapprSanitizers = methods.FindByShortName("*htmlentities*", false) + 
	methods.FindByShortName("*htmlspecialchars*", false);

CxList quotesString = All.FindByType(typeof(UnknownReference)).FindByShortName(@"ENT_QUOTES");

inapprSanitizers -= quotesString.GetAncOfType(typeof(MethodInvokeExpr));

// Look at inapprSanitizers that is affected by inputs, and affecting outputs (potential XSS),
// but does not pass through a sanitizer

CxList sanitize = Find_XSS_Sanitize() - inapprSanitizers; // XSS_Sanitize may contain some inappropriates

CxList relevantSanitizers = inapprSanitizers
	.InfluencedByAndNotSanitized(inputs, sanitize)
	.InfluencingOnAndNotSanitized(outputs, sanitize);

result = relevantSanitizers;