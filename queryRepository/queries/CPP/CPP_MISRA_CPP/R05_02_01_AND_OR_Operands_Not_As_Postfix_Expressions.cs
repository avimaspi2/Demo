/*
MISRA CPP RULE 5-2-1
------------------------------
This query searches for operands of && or || that are not postfix expressions (atomic exp, or surrounded by brackets).
except for chains of same operator

*/

// \|\|[\w\s]*[^\)\(\w\s\|] - || followed by illeagal symbol before it sees a bracket - ex. "|| b > 5"
// \|\|[^\)\(]*[^\)\(\s][^\)\(]*\( - || followed by a non space symbol before "("
// we check for the reverse order of the above cases
// and overall check for both binary ops


CxList exprOr = All.GetByBinaryOperator(BinaryOperator.BooleanOr).FindByRegex(
	@"\|\|[\w\s]*[^\)\(\w\s\|]|\|\|[^\)\(]*[^\)\(\s][^\)\(]*\(|[^\)\(\w\s\|][\w\s]*\|\||\)[^\)\(]*[^\)\(\s][^\)\(]*\|\|",
	false, false, false);

CxList exprAnd =
	All.GetByBinaryOperator(BinaryOperator.BooleanAnd).FindByRegex(
	@"\&\&[\w\s]*[^\)\(\w\s\&]|\&\&[^\)\(]*[^\)\(\s][^\)\(]*\(|[^\)\(\w\s\&][\w\s]*\&\&|\)[^\)\(]*[^\)\(\s][^\)\(]*\&\&",
	false, false, false);
	
result = exprOr + exprAnd;