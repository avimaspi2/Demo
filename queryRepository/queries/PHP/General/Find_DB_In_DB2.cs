CxList methods = Find_Methods();

//CxList db2 = methods.FindByShortName("db2_*");
// 1 - Explicite DB function names
CxList directDbMethods = methods.FindByShortNames(new List<string> {"db2_bind_param", "db2_exec", "db2_execute" });
//	db2.FindByShortName("db2_bind_param") + 
//	db2.FindByShortName("db2_exec") + 
//	db2.FindByShortName("db2_execute");
	
result.Add(directDbMethods);