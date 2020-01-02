/*
MISRA C RULE 12-5
------------------------------
This query searches for operands of && or || that are not primary expressions (atomic exp, or surrounded by brackets).
except for chains of same operator

	The Example below shows code with vulnerability: 

use_uint64 ( 9223372036854775808 );
use_uint32 ( 0x80000000 );

*/

// \|\|[\w\s]*[^\)\(\w\s\|] - || followed by illeagal symbol before it sees a bracket - ex. "|| b > 5"
// \|\|[^\)\(]*[^\)\(\s][^\)\(]*\( - || followed by a non space symbol before "("
// we check for the reverse order of the above cases
// and overall check for both binary ops

result = All.GetByBinaryOperator(BinaryOperator.BooleanOr).FindByRegex(
	@"\|\|[\w\s]*[^\)\(\w\s\|]|\|\|[^\)\(]*[^\)\(\s][^\)\(]*\(|[^\)\(\w\s\|][\w\s]*\|\||\)[^\)\(]*[^\)\(\s][^\)\(]*\|\|",
	false, false, false);
result.Add(	All.GetByBinaryOperator(BinaryOperator.BooleanAnd).FindByRegex(
	@"\&\&[\w\s]*[^\)\(\w\s\&]|\&\&[^\)\(]*[^\)\(\s][^\)\(]*\(|[^\)\(\w\s\&][\w\s]*\&\&|\)[^\)\(]*[^\)\(\s][^\)\(]*\&\&",
	false, false, false));