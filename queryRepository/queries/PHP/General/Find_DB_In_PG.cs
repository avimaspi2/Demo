CxList methods = Find_Methods();

// 1 - Explicite DB function names

CxList directDbMethods = methods.FindByShortNames(new List<string> {"pg_copy_from", "pg_delete", "pg_execute","pg_insert","pg_exec",
	"pg_lo_import","pg_lo_unlink","pg_lo_write","pg_prepare","pg_query_params","pg_query","pg_send_execute", "pg_send_prepare",
    "pg_send_query_params","pg_send_query","pg_update"});

/*
CxList directDbMethods =
	methods.FindByShortName("pg_copy_from") + 
	methods.FindByShortName("pg_delete") + 	
	methods.FindByShortName("pg_execute") + 
	methods.FindByShortName("pg_insert") + 
	methods.FindByShortName("pg_lo_import") + 
	methods.FindByShortName("pg_lo_unlink") + 
	methods.FindByShortName("pg_lo_write") + 
	methods.FindByShortName("pg_prepare") + 
	methods.FindByShortName("pg_query_params") +
	methods.FindByShortName("pg_query") +
	methods.FindByShortName("pg_send_execute") + 
	methods.FindByShortName("pg_send_prepare") + 
	methods.FindByShortName("pg_send_query_params") +
	methods.FindByShortName("pg_send_query") +
	methods.FindByShortName("pg_update");
*/	
result.Add(directDbMethods);