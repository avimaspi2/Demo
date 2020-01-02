CxList allMethods = Find_Methods();
CxList pgParamSanitizer = 
	allMethods.FindByShortName("dbx_escape_string");

result.Add(pgParamSanitizer);