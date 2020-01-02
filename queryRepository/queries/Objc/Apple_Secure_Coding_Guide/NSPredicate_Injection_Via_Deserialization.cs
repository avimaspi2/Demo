// Find NSPredicate.allowEvaluation methods which are called immediately after predicate object deserialization
// without sanitization before.
CxList vulnerableResults = All.NewCxList();
try
{
	CxList methods = Find_Methods();
	CxList allowEvaluationMethods = methods.FindByMemberAccess("NSPredicate.allowEvaluation");
	CxList predicates = allowEvaluationMethods.GetTargetOfMembers();
	CxList predicatesDefs = All.FindDefinition(predicates);
	CxList initInCoders = methods.FindByShortName("*initWithCoder:"); 
	CxList serializedObjectsDefs = initInCoders.GetFathers();
	CxList serializedPredicates = predicatesDefs * serializedObjectsDefs;
	
	// search for sanitization of predicate (sanitize should be done after definition and before allowEvaluation)
	CxList predicatesSanitization
		= All.DataInfluencingOn(allowEvaluationMethods).DataInfluencedBy(serializedPredicates)
		 .GetStartAndEndNodes(CxList.GetStartEndNodesType.AllButNotStartAndEnd);
	
	vulnerableResults = allowEvaluationMethods.NotInfluencedBy(predicatesSanitization);
}
catch (Exception error)
{
	cxLog.WriteDebugMessage(error);
}
result = vulnerableResults;