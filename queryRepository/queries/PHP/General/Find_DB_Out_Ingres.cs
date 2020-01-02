CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods = methods.FindByShortNames(new List<string> {"ingres_fetch_array", "ingres_fetch_assoc", "ingres_fetch_object",
	"ingres_fetch_proc_return", "ingres_fetch_row"});
/*
CxList ingres_fetch = methods.FindByShortName("ingres_fetch_*");
CxList directDbMethods = 
	ingres_fetch.FindByShortName("ingres_fetch_array") +
	ingres_fetch.FindByShortName("ingres_fetch_assoc") +
	ingres_fetch.FindByShortName("ingres_fetch_object") +
	ingres_fetch.FindByShortName("ingres_fetch_proc_return") +
	ingres_fetch.FindByShortName("ingres_fetch_row");
*/
result.Add(directDbMethods);