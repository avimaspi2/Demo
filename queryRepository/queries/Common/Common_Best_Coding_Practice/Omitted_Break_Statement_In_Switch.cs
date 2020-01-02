CxList AllCase = general.Find_Cases();

// Find all break statements
CxList AllBreakingStmts = Find_BreakStmt();
AllBreakingStmts.Add(Find_ContinueStmt());
AllBreakingStmts.Add(Find_ReturnStmt());
AllBreakingStmts.Add(Find_ThrowStmt());

CxList conditionalStatements = AllBreakingStmts.GetAncOfType(typeof(IfStmt));
conditionalStatements.Add(AllBreakingStmts.GetAncOfType(typeof(IterationStmt)));
conditionalStatements.Add(AllBreakingStmts.GetAncOfType(typeof(SwitchStmt)));
conditionalStatements.Add(AllBreakingStmts.GetAncOfType(typeof(LambdaExpr)));

foreach (CxList r in AllCase)
{
	Case c = r.TryGetCSharpGraph<Case>();
	CxList caseBreaks = AllBreakingStmts.GetByAncs(r);
	
	CxList caseConditionalStatements = conditionalStatements.GetByAncs(r);
	
	// Verify if statements with breaks on both sides!
	CxList caseConditionalIfStatements = caseConditionalStatements.FindByType(typeof(IfStmt));
	CxList caseConditionalBreaks = caseBreaks.GetByAncs(caseConditionalStatements); 
	CxList trueStmts = caseConditionalIfStatements.GetBlocksOfIfStatements(true);
	CxList falseStmts = caseConditionalIfStatements.GetBlocksOfIfStatements(false);	
	
	CxList trueCaseConditionalBreaks = caseConditionalBreaks.GetByAncs(trueStmts);
	CxList falseCaseConditionalBreaks = caseConditionalBreaks.GetByAncs(falseStmts);
	
	if (trueCaseConditionalBreaks.Count == falseCaseConditionalBreaks.Count) {
		caseConditionalBreaks -= caseBreaks.GetByAncs(caseConditionalIfStatements);
	}
	
	CxList caseBlockBreaks = caseBreaks - caseConditionalBreaks;
	
	try
	{
		// Only leave non-empty cases that are not "default:"
		if(!c.IsDefault && c.Statements.Count > 0 && caseBlockBreaks.Count == 0)
		{
			result.Add(c.NodeId, c);
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}