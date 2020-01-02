CxList methods = Find_Methods();

// 1 - Explicite DB function names

CxList directDbMethods = methods.FindByShortNames(new List<string> {"db2_bind_param", "db2_fetch_array", "db2_fetch_assoc",
	"db2_fetch_both","db2_fetch_object", "db2_fetch_row", "db2_result"});

/*
CxList db2 = methods.FindByShortName("db2_*");
CxList directDbMethods = 
	db2.FindByShortName("db2_bind_param") +
	db2.FindByShortName("db2_fetch_array") +
	db2.FindByShortName("db2_fetch_assoc") +
	db2.FindByShortName("db2_fetch_both") +
	db2.FindByShortName("db2_fetch_object") +
	db2.FindByShortName("db2_fetch_row") +
	db2.FindByShortName("db2_result");
*/

result.Add(directDbMethods);