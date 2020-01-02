CxList ifStmts = base.Find_Ifs();

foreach (CxList singleIf in ifStmts)
{
	try
	{
		IfStmt stmt = singleIf.TryGetCSharpGraph<IfStmt>();
		if (stmt.Condition != null)
		{
			CxList condition = All.NewCxList();
			condition.Add(stmt.Condition.NodeId, stmt.Condition);
			CxList conditionContent = All.GetByAncs(condition);
			conditionContent.Add(condition.GetTargetOfMembers()); // For cases like s.equals(s2)
			if (conditionContent.FindByType(typeof(BinaryExpr)).FindByShortName("||").Count > 0) continue;
			
			CxList strings = conditionContent.FindByType("string");
			CxList harecodedStrings = conditionContent.FindByAbstractValue(x => x is StringAbstractValue);
			CxList equal = conditionContent.FindByShortName("equals");
			equal -= equal.FindByFathers(conditionContent.FindByShortName("Not"));
			CxList equal2 = conditionContent.FindByShortName("==");
			equal2 -= equal2.FindByFathers(conditionContent.FindByShortName("Not"));
			CxList whitelistedStrings = All.NewCxList();
			
			foreach (CxList eq in equal)
			{
				// For cases like "s".equals(s)
				if ((eq.GetTargetOfMembers() * harecodedStrings).Count > 0)
					whitelistedStrings.Add(strings.GetParameters(eq));
				// For cases like s.equals("s")
				if ((conditionContent.GetParameters(eq) * harecodedStrings).Count > 0)
					whitelistedStrings.Add(eq.GetTargetOfMembers());
			}
			
			foreach (CxList eq in equal2)
			{
				// For cases like s == "S"
				if ((harecodedStrings.GetFathers() * eq).Count > 0)
					whitelistedStrings.Add(conditionContent.FindByFathers(eq) - harecodedStrings);
			}
			
			whitelistedStrings -= whitelistedStrings.GetTargetOfMembers(); // Remove objects
			whitelistedStrings = whitelistedStrings.FindByType(typeof(UnknownReference)); // Remove arrays and methods
			
			CxList inCondition = All.GetByAncs(singleIf.GetBlocksOfIfStatements(true));		
			result.Add(inCondition.FindAllReferences(whitelistedStrings));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}