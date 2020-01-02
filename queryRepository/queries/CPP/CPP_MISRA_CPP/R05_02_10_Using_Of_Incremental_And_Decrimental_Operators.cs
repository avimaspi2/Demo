/*
MISRA CPP RULE 5-2-10
------------------------------
This query searches for uses of the ++ or -- operators mixed with other operators
	The Example below shows code with vulnerability: 

use_uint16 ( mc2_1213_c++ );

while (i-- > 0){
...
}

*/


CxList ir = All.FindByType(typeof(IndexerRef));
CxList be = All.FindByType(typeof(BinaryExpr));
CxList ae = All.FindByType(typeof(AssignExpr));
CxList prefix = All.FindByType(typeof(UnaryExpr)).FindByShortName("Increment") +
	All.FindByType(typeof(UnaryExpr)).FindByShortName("Decrement");

CxList postfix = All.FindByType(typeof(PostfixExpr));
CxList postFathers = postfix.GetFathers();
CxList preFathers = prefix.GetFathers();
postFathers = postFathers * (ir + be + ae);
preFathers = preFathers * (ir + be + ae);
result.Add(preFathers + postFathers);
CxList secondDegree = postfix.GetFathers() - (postFathers + All.FindByType(typeof(ExprStmt)));
secondDegree.Add(prefix.GetFathers() - (preFathers + All.FindByType(typeof(ExprStmt))));
result.Add(secondDegree.GetFathers() * (ir + be + ae));
result -= (result.GetByBinaryOperator(BinaryOperator.BooleanAnd) +
	result.GetByBinaryOperator(BinaryOperator.BooleanOr) +
	result.GetByBinaryOperator(BinaryOperator.BitwiseOr) +
	result.GetByBinaryOperator(BinaryOperator.BitwiseAnd) +
	result.GetByBinaryOperator(BinaryOperator.BitwiseXor));