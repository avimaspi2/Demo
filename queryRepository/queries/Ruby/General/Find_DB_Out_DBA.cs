CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods = 
	methods.FindByShortName("dba_fetch") + 
	methods.FindByShortName("dba_firstkey") + 
	methods.FindByShortName("dba_nextkey") ;
result.Add(directDbMethods);