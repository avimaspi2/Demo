CxList methods = Find_Methods();

// 1 - Direct DB function names
CxList directDbMethods = methods.FindByShortNames(new List<string> {"ifx_get_blob", "ifx_get_char", "ifx_fetch_row",
	"ifxus_read_slob"});
/*
CxList ifx = methods.FindByShortName("ifx*");
CxList directDbMethods =
	ifx.FindByShortName("ifx_get_blob") +
	ifx.FindByShortName("ifx_get_char") +
	ifx.FindByShortName("ifx_fetch_row") +
	ifx.FindByShortName("ifxus_read_slob");
*/	
	result.Add(directDbMethods);