if (param.Length == 2)
{
	try
	{
		bool isPotential = (bool) param[1];
		
		CxList evilStrings = param[0] as CxList;

		if (evilStrings.Count > 0)
		{

			CxList matchStringsFromInputs = Find_Match_Strings_From_Inputs();
			CxList allMatchStrings = Find_Strings_In_Match();
			CxList inputs = Find_Inputs();
			
			CxList matchString = All.NewCxList();
			if (isPotential)
			{
				matchString = allMatchStrings - matchStringsFromInputs - inputs;
			}
			else
			{
				matchString = matchStringsFromInputs + 
					((allMatchStrings + allMatchStrings.GetTargetOfMembers()) * inputs);
			}
		
			// Find all matches of regexes
			CxList match = Find_Match();

			// Leave only relevant matches (strings influenced by input)
			match = match.FindByParameters(matchString);

			if (match.Count > 0)
			{
				// Sanitization
				CxList sanitize = Find_Integers();

				// Find all regex commands and patterns
				CxList regex = Find_Regex();
				CxList regexIsMatch = Find_Methods().FindByMemberAccess("regex.isMatch", false);
				CxList regexPatterns = All.GetByAncs(All.GetParameters(Find_Regex(), 0));
				regexPatterns.Add(All.GetByAncs(All.GetParameters(regexIsMatch, 1)));

				// Find regex commands that are influenced by evil strings
				CxList activeEvilRegexes = (evilStrings * regexPatterns) + 
					evilStrings.InfluencingOnAndNotSanitized(regexPatterns, sanitize);

				// Find relevant matches
				result = match.InfluencedByAndNotSanitized(activeEvilRegexes, sanitize + allMatchStrings);
				result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);
			}
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}