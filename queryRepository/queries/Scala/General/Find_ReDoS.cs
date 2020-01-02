if (param.Length == 3)
{
	try
	{
		// Find evil strings
		CxList evilStrings = param[0] as CxList;

		// Find match command
		CxList match = param[1] as CxList; 
		
		bool isPotential = (bool) param[2];
        
		if (match.Count > 0 && evilStrings.Count > 0)
		{
			// Find all regex patterns
			CxList underMatch = All.GetByAncs(match);
			
			// Methods without regex arguments
			CxList methodsWithoutRegexParams = All.NewCxList();
			methodsWithoutRegexParams.Add(match.FindByShortNames(new List<string>{"matcher", "replacePattern"}));
			methodsWithoutRegexParams.Add(match.FindByMemberAccess("Pattern.split"));
			methodsWithoutRegexParams.Add(match.FindByMemberAccess("Regex.*"));
			methodsWithoutRegexParams.Add(match.FindByMemberAccess("Matcher.replace*"));
			
			CxList matchParams = underMatch.GetParameters(match - methodsWithoutRegexParams, 0);
			
			// StringUtils.replacePattern holds the regex in the second argument
			// Pattern.matcher doesn't have regexes in its parameters
			CxList replacePattern = match.FindByShortName("replacePattern");
			matchParams.Add(underMatch.GetParameters(replacePattern, 1));
			
			CxList regexPatterns = underMatch.GetByAncs(matchParams);

			regexPatterns = regexPatterns.FindByType(typeof(UnknownReference));
			regexPatterns.Add(Find_Strings());
			regexPatterns.Add(regexPatterns.FindByType(typeof(MemberAccess)));
			regexPatterns.Add(methodsWithoutRegexParams.GetTargetOfMembers());
		
			// Sanitization
			CxList sanitize = Find_Integers();
			sanitize -= All.FindByShortName("matches");
			
			CxList activeEvilRegexesSanitize = All.NewCxList();
			activeEvilRegexesSanitize.Add(sanitize);
			activeEvilRegexesSanitize.Add(All.GetParameters(Find_Methods().FindByMemberAccess("Pattern.matcher")));
			
			// Find regex commands that are influenced by evil strings
			CxList activeEvilRegexes = (evilStrings * regexPatterns); 
			activeEvilRegexes.Add(evilStrings.InfluencingOnAndNotSanitized(regexPatterns, activeEvilRegexesSanitize));
			
			// Inputs
			CxList inputs = Find_Inputs();
			if (isPotential)
			{
				match -= match.InfluencedByAndNotSanitized(inputs, sanitize);
			}
			else
			{
				match = match.InfluencedByAndNotSanitized(inputs, sanitize);
			}
			// Find relevant matches
			match -= match.FindByShortName("compile");
			result = match.InfluencedByAndNotSanitized(activeEvilRegexes, sanitize)
				.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}