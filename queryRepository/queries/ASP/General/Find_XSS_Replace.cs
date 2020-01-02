CxList replace = All.FindByShortName("replace*");

replace = replace.FindByType(typeof(MemberAccess)) + 
	replace.FindByType(typeof(MethodInvokeExpr));

result = replace.FindByParameterValue(0, "\"<\"", BinaryOperator.IdentityEquality)
	.FindByParameterValue(1, "\"<\"", BinaryOperator.IdentityInequality)
	+
	replace.FindByParameterValue(1, "\"<\"", BinaryOperator.IdentityEquality)
	.FindByParameterValue(0, "\"<\"", BinaryOperator.IdentityInequality)
	.FindByParameterValue(2, "\"<\"", BinaryOperator.IdentityInequality);

CxList replaced = replace.FindByParameterValue(0, "\">\"", BinaryOperator.IdentityEquality)
	.FindByParameterValue(1, "\">\"", BinaryOperator.IdentityInequality)
	+
	replace.FindByParameterValue(1, "\">\"", BinaryOperator.IdentityEquality)
	.FindByParameterValue(0, "\">\"", BinaryOperator.IdentityInequality)
	.FindByParameterValue(2, "\">\"", BinaryOperator.IdentityInequality);

result.Add(replaced);

CxList repl1 = Find_Methods().FindByShortName("replace");
repl1 = All.GetByAncs(All.GetParameters(repl1, 1));
CxList repl2 = Find_Methods().FindByShortName("replaceall");
repl2 = All.GetByAncs(All.GetParameters(repl2, 0));

result = All.GetByAncs(result).FindByShortName("replace*", false) + repl1 + repl2;