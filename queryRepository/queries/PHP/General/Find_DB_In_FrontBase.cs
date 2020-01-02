CxList methods = Find_Methods();

// 1 - Explicite DB function names
//CxList fbsql = methods.FindByShortName("fbsql_*");
CxList directDbMethods = methods.FindByShortNames(new List<String>(){ "fbsql_bind_param", "fbsql_query" });
	
result.Add(directDbMethods);