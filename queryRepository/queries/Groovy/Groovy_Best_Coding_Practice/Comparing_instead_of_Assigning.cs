CxList assign = All.FindByType(typeof(AssignExpr));
CxList condition = Find_Conditions();

CxList compare = All.FindByShortName("==");
compare -= condition;
compare -= compare.GetByAncs(All.FindByType(typeof(Param)));

CxList compareFather = compare.GetFathers();
CxList statementsWithCompare = 
	compareFather.FindByType(typeof(StatementCollection)) +
	compareFather.FindByType(typeof(CatchCollection));

result = compare * compare.GetFathers() + // No sense of comparing the compare result, must be a mistake
	compare.FindByFathers(statementsWithCompare); // No sense of having a compare in a collection