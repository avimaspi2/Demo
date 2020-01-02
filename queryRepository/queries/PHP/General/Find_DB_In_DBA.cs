CxList methods = Find_Methods();

// 1 - Explicite DB function names
//CxList dba = methods.FindByShortName("dba_*");
CxList directDbMethods = methods.FindByShortNames(new List<String>(){ "dba_insert", "dba_replace" });
	
result.Add(directDbMethods);