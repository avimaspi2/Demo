if (param.Length == 3)
{
	try
	{
		bool isPotential = (bool) param[2];
		
		// Inputs
		CxList inputs = Find_Inputs();

		// Find evil strings
		CxList evilStrings = param[0] as CxList;

		// Find match command
		CxList match = param[1] as CxList;//Find_Match() + Find_Replace();

		// Find all regex patterns
		CxList underMatch = All.GetByAncs(match);
		
		CxList binaryExpr = match.FindByType(typeof(BinaryExpr));
		CxList rightSide = All.NewCxList();

		// Get the right side of ==~ operator
		foreach(CxList t in binaryExpr)
		{
			BinaryExpr be = t.data.GetByIndex(0) as BinaryExpr;
			rightSide.Add(be.Right.NodeId, be.Right);
		}
		
		CxList regexPatterns = underMatch.GetByAncs(underMatch.GetParameters(match, 0))
			+ underMatch.GetByAncs(rightSide);
		
		regexPatterns = regexPatterns.FindByType(typeof(UnknownReference)) +
			regexPatterns.FindByType(typeof(StringLiteral)) +
			regexPatterns.FindByType(typeof(MemberAccess));

		// Sanitization
		CxList sanitize = Find_Integers();
		sanitize -= All.FindByShortName("matches");
		sanitize.Add(Find_Dead_Code_Contents());
		
		// Find regex commands that are influenced by evil strings
		CxList activeEvilRegexes = (evilStrings * regexPatterns) + 
			evilStrings.InfluencingOnAndNotSanitized(regexPatterns, sanitize);

		//Get the matched strings that are influenced by input
		CxList matchString = regexPatterns.DataInfluencedBy(inputs) + 
			((regexPatterns + regexPatterns.GetTargetOfMembers()) * inputs);
		
		// Leave only relevant matches (strings influenced by input)
		match = match.FindByParameters(matchString) +
			match.FindByType(typeof(BinaryExpr));

		if (isPotential)
		{
			match -= match.InfluencedByAndNotSanitized(inputs, sanitize);
		}
		else
		{
			match = match.InfluencedByAndNotSanitized(inputs, sanitize);
		}
		// Find relevant matches
		result = match.InfluencedByAndNotSanitized(activeEvilRegexes, sanitize + regexPatterns)
			.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}