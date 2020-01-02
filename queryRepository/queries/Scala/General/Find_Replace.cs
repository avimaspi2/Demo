CxList replace = All.FindByShortName("replace*", false);

replace = replace.FindByType(typeof(MemberAccess)) + 
	replace.FindByType(typeof(MethodInvokeExpr));

result = replace.FindByParameterValue(0, "\"'\"", BinaryOperator.IdentityEquality)
	.FindByParameterValue(1, "\"'\"", BinaryOperator.IdentityInequality)
	+
	replace.FindByParameterValue(1, "\"'\"", BinaryOperator.IdentityEquality)
	.FindByParameterValue(0, "\"'\"", BinaryOperator.IdentityInequality)
	.FindByParameterValue(2, "\"'\"", BinaryOperator.IdentityInequality);

//remove already found
replace -= result;

foreach(CxList x in replace) {
	string str1 = x.GetLeftmostTarget().GetName();

	if(str1.Equals("'") && x.FindByParameterValue(1, "\"'\"", BinaryOperator.IdentityInequality).Count > 0) {
		result.Add(x);
	}
}

result = All.GetByAncs(result).FindByShortName("replace*", false);