CxList replace = All.FindByShortName("replace*");
replace = replace.FindByType(typeof(MemberAccess)) + 
	replace.FindByType(typeof(MethodInvokeExpr));

result = replace.FindByParameterValue(1, "\"'\"", 
	BinaryOperator.IdentityEquality).FindByParameterValue(2, "\"'\"", BinaryOperator.IdentityInequality);

result = All.GetByAncs(result).FindByShortName("replace*", false);