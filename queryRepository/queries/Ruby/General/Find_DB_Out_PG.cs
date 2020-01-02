CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods = 
	methods.FindByShortName("pg_copy_to") + 
	methods.FindByShortName("pg_fetch_all_columns") + 
	methods.FindByShortName("pg_fetch_all") +
	methods.FindByShortName("pg_fetch_assoc") +
	methods.FindByShortName("pg_fetch_object") + 
	methods.FindByShortName("pg_fetch_row") + 
	methods.FindByShortName("pg_get_result") +
	methods.FindByShortName("pg_lo_read") ;
result.Add(directDbMethods);