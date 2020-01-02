CxList methods = Find_Methods();

// 1 - Direct DB function names
//CxList msql = methods.FindByShortName("msql*");
CxList directDbMethods = methods.FindByShortNames(new List<string> {"msql", "msql_query", "msql_db_query" });
/*
CxList directDbMethods =
	msql.FindByShortName("msql") +
	msql.FindByShortName("msql_query") +
	msql.FindByShortName("msql_db_query");
*/
result.Add(directDbMethods);