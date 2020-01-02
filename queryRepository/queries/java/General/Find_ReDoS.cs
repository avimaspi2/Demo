/* this Function gets 4 parameter:
1) The 'evilStrings' - meaning, the strings we consider to be dangrouse
2) The functions in which to look for those strings as parameters
3) In which parameter to we expect the regex? (this parameter is an int)
4) A boolean. Gets true so that the query returns definate ReDoS. False so that it return potential ReDoS
*/

if (param.Length == 4)
{
	try
	{
		// Find evil strings
		CxList evilStrings = param[0] as CxList;

		// Find match command
		CxList match = param[1] as CxList;//Find_Match() + Find_Replace();

		int parametersPosition = (int) param[2];
		
		bool isPotential = (bool) param[3];
		
		if (match.Count > 0 && evilStrings.Count > 0)
		{
			// Find all regex patterns
			CxList underMatch = All.GetByAncs(match);
			CxList allRegexPatterns = underMatch.GetByAncs(underMatch.GetParameters(match, parametersPosition));
			
			CxList regexPatterns = Find_Strings();
			regexPatterns.Add(allRegexPatterns.FindByType(typeof(UnknownReference)));
			regexPatterns.Add(allRegexPatterns.FindByType(typeof(MemberAccess)));

			// Sanitization
			CxList sanitize = Find_Integers();
			sanitize -= All.FindByShortName("matches");
			sanitize.Add(Find_Dead_Code_Contents());
		
			// Find regex commands that are influenced by evil strings
			CxList activeEvilRegexes = (evilStrings * regexPatterns); 
			activeEvilRegexes.Add(evilStrings.InfluencingOnAndNotSanitized(regexPatterns, sanitize).
				GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

			// Leave only relevant matches (strings influenced by evil strings)
			match = match.FindByParameters(activeEvilRegexes);

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
			result = match.InfluencedByAndNotSanitized(activeEvilRegexes, sanitize)
				.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}