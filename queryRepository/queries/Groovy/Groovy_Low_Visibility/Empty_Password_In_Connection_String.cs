CxList sql_methods = Find_Methods().FindByMemberAccess("Sql.newInstance") +
	Find_Methods().FindByMemberAccess("DriverManager.getConnection");
CxList empty_strings = Find_Empty_Strings();
CxList password_parameters = All.GetParameters(sql_methods, 2);
CxList parameters_empty_strings = All.GetParameters(sql_methods.FindByParameterValue(2, "\"\"", BinaryOperator.IdentityEquality), 2);

result = empty_strings.InfluencingOn(password_parameters) + 
	parameters_empty_strings.FindByType(typeof(StringLiteral));