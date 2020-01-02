CxList methods = Find_Methods();

// 1 - Direct DB function names
CxList directDbMethods = methods.FindByShortNames(new List<string> {"ovrimos_result_all", "ovrimos_result", 
	"ovrimos_fetch_into", "ovrimos_fetch_row"});
/*
CxList ovrimos = methods.FindByShortName("ovrimos_*");
CxList directDbMethods =
	ovrimos.FindByShortName("ovrimos_result_all") +
	ovrimos.FindByShortName("ovrimos_result") +
	ovrimos.FindByShortName("ovrimos_fetch_into") +
	ovrimos.FindByShortName("ovrimos_fetch_row");
*/	
	result.Add(directDbMethods);