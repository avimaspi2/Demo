CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods = methods.FindByShortNames(new List<string> {"pg_copy_to", "pg_fetch_all_columns", 
	"pg_fetch_all", "pg_fetch_array", "pg_fetch_assoc","pg_fetch_object","pg_fetch_result","pg_fetch_row",
	"pg_get_result","pg_lo_export","pg_lo_read","pg_lo_read_all"});
/*
CxList directDbMethods = 
	methods.FindByShortName("pg_copy_to") + 
	methods.FindByShortName("pg_fetch_all_columns") + 
	methods.FindByShortName("pg_fetch_all") +
	methods.FindByShortName("pg_fetch_array") +
	methods.FindByShortName("pg_fetch_assoc") +
	methods.FindByShortName("pg_fetch_object") + 
	methods.FindByShortName("pg_fetch_result") + 
	methods.FindByShortName("pg_fetch_row") + 
	methods.FindByShortName("pg_get_result") +
	methods.FindByShortName("pg_lo_export") +
	methods.FindByShortName("pg_lo_read") + 
	methods.FindByShortName("pg_lo_read_all");
*/
result.Add(directDbMethods);