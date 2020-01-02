CxList methods = Find_Methods();

result.Add(Find_General_Sanitize());

result.Add(Find_Left_Side_Sanitize());
result.Add(Find_Replace());
result.Add(Find_String_Sanitize());

result.Add(methods.FindByShortNames(new List<string>
	{"urlencode", "filter_var", "addslashes", "array_walk_recursive","array_walk","apply_filters",	

		// WordPress sanitizers
		"esc_sql","sanitize_title","apply_filters","add_magic_quotes","get_gmt_from_date",
		"wp_hash","wp_cache_get","wp_nonce_url","wpsc_validate_customer_cookie"}) +
   
	All.FindByShortName("wpdb").GetMembersOfTarget().FindByShortName("Escape", false));
	
result.Add(Find_Zend_Sanitize() + Find_Kohana_Sanitize() + Find_Cake_Sanitize());
result.Add(Find_File_Access());

CxList call_user_func = methods.FindByShortNames(new List<string> {"call_user_func", "call_user_func_array"});

CxList user_func_first_param = methods.GetParameters(call_user_func, 0);
user_func_first_param -= user_func_first_param.FindByShortName("array", false);
CxList parameters = All.GetParameters(call_user_func);
CxList user_func_params = parameters - parameters.GetByAncs(user_func_first_param);
result.Add(user_func_params);

//WordPress prepared statement parameters (without the first parameter)
CxList wordpressPrepare = All.FindByShortName("*db").GetMembersOfTarget().FindByShortName("prepare");
CxList wordpressPrepareParams = All.GetParameters(wordpressPrepare) - All.GetParameters(wordpressPrepare, 0);
result.Add(wordpressPrepareParams);
result.Add(Find_Doctrine_Sanitize());

// This section will be growing up from version to version.
// for functions below (like fetchOne), all parameters except first one will be sanitizers
CxList paramAsSanitizer = All.GetParameters(Find_DB_In().FindByShortName("*fetchOne")) - 
	All.GetParameters(Find_DB_In().FindByShortName("*fetchOne"), 0);

result.Add(paramAsSanitizer);