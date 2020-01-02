CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods = 
	//methods.FindByShortName("mysql_query") +
	//methods.FindByShortName("mysql_db_query") +
	methods.FindByShortName("mysql_fetch_array") +
	methods.FindByShortName("mysql_fetch_assoc") +
	methods.FindByShortName("mysql_fetch_field") +
	methods.FindByShortName("mysql_fetch_object") +
	methods.FindByShortName("mysql_fetch_row") +
	methods.FindByShortName("mysql_result") +
	methods.FindByShortName("mysql_unbuffered_query") +	
	methods.FindByShortName("mysqli_reap_async_query") +
	methods.FindByShortName("reap_async_query") +
	//methods.FindByShortName("mysql_store_result") +
	//methods.FindByShortName("store_result") +
	methods.FindByShortName("mysqli_bind_result") +
	methods.FindByShortName("bind_result") +
	methods.FindByShortName("mysqli_result_fetch_all") +
	methods.FindByShortName("fetch_all") +
	methods.FindByShortName("mysqli_result_retch_array") +
	methods.FindByShortName("retch_array") +
	methods.FindByShortName("mysqli_result_fetch_assoc") +
	methods.FindByShortName("fetch_assoc") +
	methods.FindByShortName("mysqli_result_fetch_object") +
	methods.FindByShortName("fetch_object") +
	methods.FindByShortName("mysqli_result_fetch_row") + 
	methods.FindByShortName("fetch_row") +
	methods.FindByShortName("store_result")
	;
result.Add(directDbMethods);