CxList methods = Find_Methods();

CxList directDbMethods = methods.FindByShortNames(new List<string> {"msql_fetch_array", "msql_fetch_field", "msql_fetch_object",
	"msql_fetch_row","msql_result","msql_tablename","msql_dbname"});
/*
CxList msql = methods.FindByShortName("msql_*");
CxList msql_fetch = msql.FindByShortName("msql_fetch_*");
CxList directDbMethods =
	msql_fetch.FindByShortName("msql_fetch_array") +
	msql_fetch.FindByShortName("msql_fetch_field") +
	msql_fetch.FindByShortName("msql_fetch_object") +
	msql_fetch.FindByShortName("msql_fetch_row") +
	msql.FindByShortName("msql_result") + 
	msql.FindByShortName("msql_tablename") +
	msql.FindByShortName("msql_dbname");
*/
result.Add(directDbMethods);