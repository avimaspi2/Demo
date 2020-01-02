CxList allMethods = Find_Methods();
CxList mysqlParamSanitizer = 
	/*allMethods.FindByShortName("mysql_escape_string") +
	allMethods.FindByShortName("mysql_real_escape_string") +
	allMethods.FindByShortName("real_escape_string") +
	allMethods.FindByShortName("mysqli_real_escape_string") +
	allMethods.FindByShortName("bind_param") +
	allMethods.FindByShortName("mysqli_stmt_bind_param") +
	allMethods.FindByShortName("mysqli_bind_param") +
	allMethods.FindByShortName("mysqli_escape_string") +*/
	allMethods.FindByShortName("escape_string") +
	allMethods.FindByShortName("quote");

result = mysqlParamSanitizer;