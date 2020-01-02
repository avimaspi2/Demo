CxList replace = All.FindByMemberAccess("*.replace");

result = replace.FindByParameterValue(0, "<", BinaryOperator.IdentityEquality)
	.FindByParameterValue(1, "<", BinaryOperator.IdentityInequality);

result.Add(replace.FindByParameterValue(1, "<", BinaryOperator.IdentityEquality)
	.FindByParameterValue(0, "<", BinaryOperator.IdentityInequality)
	.FindByParameterValue(2, "<", BinaryOperator.IdentityInequality));

result.Add(replace.FindByParameterValue(0, ">", BinaryOperator.IdentityEquality)
	.FindByParameterValue(1, ">", BinaryOperator.IdentityInequality));

result.Add(replace.FindByParameterValue(1, ">", BinaryOperator.IdentityEquality)
	.FindByParameterValue(0, ">", BinaryOperator.IdentityInequality)
	.FindByParameterValue(2, ">", BinaryOperator.IdentityInequality));