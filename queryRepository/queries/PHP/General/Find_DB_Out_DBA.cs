CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods = methods.FindByShortNames(new List<string> {"dba_fetch", "dba_firstkey", "dba_nextkey"});
/*
CxList dba = methods.FindByShortName("dba_*");
CxList directDbMethods = 
	dba.FindByShortName("dba_fetch") + 
	dba.FindByShortName("dba_firstkey") + 
	dba.FindByShortName("dba_nextkey");
*/
result.Add(directDbMethods);