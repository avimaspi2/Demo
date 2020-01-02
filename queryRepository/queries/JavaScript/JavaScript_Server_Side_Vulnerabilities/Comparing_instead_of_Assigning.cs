/*
	find all comparisons statements that are not in condition 
	find all StatementCollection, CatchCollection and ExprStmt that wraps comparisons statements
	return the comparisons that are in those collections
*/
CxList compare = All.FindByShortName("==");

CxList _toRemove = NodeJS_Find_Conditions_Parameters();
_toRemove.Add(compare.GetByAncs(Find_Param()));

compare -= _toRemove;

CxList compareFather = compare.GetFathers();

CxList statementsWithCompare = 
	compareFather.FindByType(typeof(StatementCollection));
statementsWithCompare.Add(compareFather.FindByType(typeof(CatchCollection)));
statementsWithCompare.Add(compareFather.FindByType(typeof(ExprStmt)));
	

result = compare * compare.GetFathers(); // No sense of comparing the compare result, must be a mistake
result.Add(compare.FindByFathers(statementsWithCompare)); // No sense of having a compare in a collection