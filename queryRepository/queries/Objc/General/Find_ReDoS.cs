if (param.Length == 2)
{
	try
	{
			
		bool isPotential = (bool) param[1];
			
		CxList evilStrings = param[0] as CxList;
	
		if (evilStrings.Count > 0)
		{
			CxList findMatchResults = Find_Match();
			CxList inputs = Find_Inputs();
			CxList methods = Find_Methods();			
			
			CxList match = All.NewCxList();
			
			CxList allMatchs = findMatchResults.GetTargetOfMembers() * inputs;			
			allMatchs.Add(findMatchResults);
			
			if (isPotential)
			{
				match = findMatchResults - allMatchs;
			}
			else
			{
				match.Add(allMatchs);
			}
	
			if (match.Count > 0)
			{
				// Find all regex patterns
				CxList regex = Find_Regex();
				CxList regexIOSMatch = match - regex; // IOS regex methods
				CxList underMatch = All.GetByAncs(regexIOSMatch);
				CxList regexPatternsPreFilter = underMatch.GetByAncs(underMatch.GetParameters(regexIOSMatch, 0));
				regexPatternsPreFilter.Add(regexIOSMatch - regexIOSMatch.FindByType(typeof(MethodInvokeExpr)));
				// Filter
				CxList regexPatterns = regexPatternsPreFilter.FindByType(typeof(UnknownReference));
				regexPatterns.Add(regexPatternsPreFilter.FindByType(typeof(StringLiteral)));
				regexPatterns.Add(regexPatternsPreFilter.FindByType(typeof(MemberAccess)));
			
				// Sanitization
				CxList sanitize = Find_Integers();
				sanitize.Add(regexPatterns.GetParameters(methods.FindByShortName(@"predicateWithFormat:*"), 0));
				
				// Find all regex commands and patterns
				string[] regCompNames = {"regcomp", "regncomp", "regwcomp", "regwncomp", "regcomp_l", "regncomp_l", "regwcomp_l", "regwncomp_l"};
				CxList regexIsMatch = methods.FindByShortNames(new List<string>(regCompNames));
				regexPatterns.Add(All.GetByAncs(All.GetParameters(regex, 0)));
				regexPatterns.Add(All.GetByAncs(All.GetParameters(regexIsMatch, 1)));
	
				// Find regex commands that are influenced by evil strings
				CxList activeEvilRegexes = (evilStrings * regexPatterns);
				activeEvilRegexes.Add(evilStrings.InfluencingOnAndNotSanitized(regexPatterns, sanitize));
				
				CxList activeEvilRegexesLastNode = activeEvilRegexes.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartAndEndNodes);
				CxList activeEvilRegexesMethodInvokes = activeEvilRegexesLastNode.GetAncOfType(typeof(MethodInvokeExpr));
				CxList activeEvilRegexes2 = All.GetParameters(activeEvilRegexesMethodInvokes.GetByAncs(regexIsMatch), 0);
				
				// Find relevant matches
				result = match.InfluencedByAndNotSanitized(activeEvilRegexes, sanitize);
				result.Add(match.InfluencedByAndNotSanitized(All.FindAllReferences(activeEvilRegexes2), sanitize));
				
				// Add predicates which execute a match and are compromised:
				result.Add(match.FindByShortName(@"predicateWithFormat:*").InfluencedByAndNotSanitized(inputs, sanitize));
				
				CxList nsPredicateList = methods.FindByShortName("NSPredicate*");
				CxList inputsDeclarators = inputs.GetAncOfType(typeof(Declarator));
				CxList stringMatches = Find_Strings().FindByShortName("*MATCHES*");
				result.Add(nsPredicateList.InfluencedBy(stringMatches).InfluencedBy(inputsDeclarators));
				
				result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
				
				// Add IOS predicates with compromised patterns
				string[] initMethodNames = {@"regularExpressionWithPattern:options:error:", @"initWithPattern:options:error:"};
				CxList initMethods = methods.FindByShortNames(new List<string>(initMethodNames));
				CxList compromisedInitMethods = initMethods.InfluencedByAndNotSanitized(evilStrings, sanitize);
				CxList compromisedRegex = regex.InfluencedBy(compromisedInitMethods);
				result.Add(All.FindAllReferences(compromisedRegex).GetMembersOfTarget() * match);
			}
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}