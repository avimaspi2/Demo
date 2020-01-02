CxList methods = Find_Methods();

// 1 - Explicite DB function names
//CxList ingres = methods.FindByShortName("ingres_*");
CxList directDbMethods = methods.FindByShortNames(new List<string> {"ingres_execute", "ingres_prepare", "ingres_query" });
//	ingres.FindByShortName("ingres_execute") +
//	ingres.FindByShortName("ingres_prepare") +
//	ingres.FindByShortName("ingres_query");
	
result.Add(directDbMethods);