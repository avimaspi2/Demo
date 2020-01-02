CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods =
	methods.FindByShortName("pg_execute") + 
	methods.FindByShortName("pg_prepare") + 
	methods.FindByShortName("pg_query_params") +
	methods.FindByShortName("pg_query") +
	methods.FindByShortName("pg_send_execute") + 
	methods.FindByShortName("pg_send_prepare") + 
	methods.FindByShortName("pg_send_query_params") +
	methods.FindByShortName("pg_send_query");

// 2 - "PG::Connection" methods
CxList pgConnectionMethods = methods.FindByMemberAccess("PG.Connection", "exec");
pgConnectionMethods.Add(methods.FindByMemberAccess("PG.Connection", "exec_params"));
pgConnectionMethods.Add(methods.FindByMemberAccess("PG.Connection", "exec_prepared"));
	
result.Add(directDbMethods);
result.Add(pgConnectionMethods);