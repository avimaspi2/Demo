CxList methods = Find_Methods();
CxList ora=methods.FindByShortName("ora_fetch*");
// 1 - Direct DB function names
CxList directDbMethods =
	ora.FindByShortName("ora_fetch_into") +
	ora.FindByShortName("ora_fetch");
	
	result.Add(directDbMethods);