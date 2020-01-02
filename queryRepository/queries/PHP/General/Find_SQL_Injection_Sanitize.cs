//sanitizer for SQL related queries
CxList methods = Find_Methods();
result = Find_Sanitize();
result.Add(Find_Sanitize_ADODB());
result.Add(Find_Sanitize_DB2());
result.Add(Find_Sanitize_DBX());
result.Add(Find_Sanitize_Ingres());
result.Add(Find_Sanitize_MDB2());
result.Add(Find_Sanitize_MaxDB());
result.Add(Find_Sanitize_MSSQL());
result.Add(Find_Sanitize_MYSQL());
result.Add(Find_Sanitize_ODBC());
result.Add(Find_Sanitize_Ora());
result.Add(Find_Sanitize_ORACLE());
result.Add(Find_Sanitize_PDO());
result.Add(Find_Sanitize_PG());
result.Add(Find_Sanitize_SQLite());
result.Add(Find_Sanitize_Mongo());
result.Add(Find_Sanitize_Symfony());
result.Add(Find_Sanitize_Creole());
result.Add(Find_HTML_Encode());

// Json_Encode may be SQLi sanitizer if second parameter JSON_HEX_APOS used
CxList json_encodeMethods = methods.FindByShortName("json_encode");
CxList secondParam = All.GetParameters(json_encodeMethods, 1);
secondParam = All.GetByAncs(secondParam).FindByType(typeof(UnknownReference));
CxList secondParamJSON_HEX_APOS = secondParam.FindByShortName("JSON_HEX_APOS", false);
CxList json_encodeAsSQLiSanitizer = secondParamJSON_HEX_APOS.GetAncOfType(typeof(MethodInvokeExpr));
result.Add(json_encodeAsSQLiSanitizer);

// Some methods considered sanitizers in php
result.Add(methods.FindByShortNames(new List<string> {
		"urlencode", "rawurlencode"}, false));