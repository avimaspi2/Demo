CxList methods = Find_Methods();
CxList ifx = methods.FindByShortName("ifx*");
// 1 - Explicite DB function names
CxList directDbMethods = ifx.FindByShortNames(new List<string> 
	{"ifxus_write_slob", "ifx_update_blob", "ifx_prepare", "ifx_query", "ifx_do" });
/*	
	ifx.FindByShortName("ifxus_write_slob") +
	ifx.FindByShortName("ifx_update_blob") +
	ifx.FindByShortName("ifx_prepare") +
	ifx.FindByShortName("ifx_query") +
	ifx.FindByShortName("ifx_do");
*/	
result.Add(directDbMethods);