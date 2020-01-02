CxList condition = Find_Conditions();

CxList compare = All.FindByShortName("==");
compare -= condition;
compare -= compare.GetByAncs(Find_Param());

CxList compareFather = compare.GetFathers();
CxList statementsWithCompare = compareFather.FindByType(typeof(StatementCollection));
statementsWithCompare.Add(compareFather.FindByType(typeof(CatchCollection)));

result = compare * compare.GetFathers(); // No sense of comparing the compare result, must be a mistake
result.Add(compare.FindByFathers(statementsWithCompare)); // No sense of having a compare in a collection