CxList replace = All.FindByName("*.Replace*");

replace = replace.FindByType(typeof(MemberAccess)) + 
	replace.FindByType(typeof(MethodInvokeExpr));

result = replace.FindByParameterValue(0, "'", BinaryOperator.IdentityEquality)
	.FindByParameterValue(1, "'", BinaryOperator.IdentityInequality)
	+
	replace.FindByParameterValue(1, "'", BinaryOperator.IdentityEquality)
	.FindByParameterValue(0, "'", BinaryOperator.IdentityInequality)
	.FindByParameterValue(2, "'", BinaryOperator.IdentityInequality);