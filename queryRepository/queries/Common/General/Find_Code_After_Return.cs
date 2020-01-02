CxList returnCommand = Find_ReturnStmt();
returnCommand.Add(Find_BreakStmt());
returnCommand.Add(Find_ContinueStmt());


// Add the (unlikely to exist) case of if(true) return;
CxList True = Find_True_Abstract_Value();
CxList alwaysTrueReturn = True.GetByAncs(returnCommand.GetAncOfType(typeof(IfStmt))).GetFathers();
alwaysTrueReturn = All.GetByAncs(alwaysTrueReturn) - returnCommand;
alwaysTrueReturn -= alwaysTrueReturn.GetByAncs(Find_Conditions());
alwaysTrueReturn -= alwaysTrueReturn.FindByType(typeof(ExprStmt));
alwaysTrueReturn -= alwaysTrueReturn.FindByType(typeof(BreakStmt));
alwaysTrueReturn -= alwaysTrueReturn.FindByType(typeof(StatementCollection));

foreach (CxList alwaysTrue in alwaysTrueReturn)
{
	if (alwaysTrueReturn.GetByAncs(alwaysTrue).Count == 1)
	{
		returnCommand.Add(alwaysTrue);
	}
}

result = returnCommand.GetFollowingStatements();