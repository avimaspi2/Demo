CxList allMethods = Find_Methods();
CxList mParamSanitizer = 
	allMethods.FindByShortName("mssql_bind") +
	allMethods.FindByShortName("mssql_execute");

result.Add(mParamSanitizer);