CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList fbsql = methods.FindByShortName("fbsql_*");

CxList directDbMethods = fbsql.FindByShortNames(new List<string> {"fbsql_fetch_array", "fbsql_fetch_assoc", "fbsql_fetch_field",
	"fbsql_fetch_lengths","fbsql_fetch_object", "fbsql_fetch_row", "fbsql_result"});

/*
CxList fbsqlFetch = fbsql.FindByShortName("fbsql_fetch_*");
CxList directDbMethods =
	fbsqlFetch.FindByShortName("fbsql_fetch_array") +
	fbsqlFetch.FindByShortName("fbsql_fetch_assoc") +
	fbsqlFetch.FindByShortName("fbsql_fetch_field") +
	fbsqlFetch.FindByShortName("fbsql_fetch_lengths") +
	fbsqlFetch.FindByShortName("fbsql_fetch_object") +
	fbsqlFetch.FindByShortName("fbsql_fetch_row") +
	fbsql.FindByShortName("fbsql_result");
*/
result.Add(directDbMethods);