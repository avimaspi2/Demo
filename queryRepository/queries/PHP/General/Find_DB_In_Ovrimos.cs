CxList methods = Find_Methods();

// 1 - Direct DB function names
CxList directDbMethods = methods.FindByShortNames(new List<string> {"ovrimos_exec", "ovrimos_execute", "ovrimos_prepare" });
/*
CxList ovrimos = methods.FindByShortName("ovrimos_*");
CxList directDbMethods =
	ovrimos.FindByShortName("ovrimos_exec") +
	ovrimos.FindByShortName("ovrimos_execute ") +
	ovrimos.FindByShortName("ovrimos_prepare");
*/	
	result.Add(directDbMethods);