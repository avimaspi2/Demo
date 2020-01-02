CxList searchData = All.FindByShortName("wpdb").GetMembersOfTarget();

result = searchData.FindByShortNames(new List<string> {"get_results", "get_var", "get_row", "get_col", "query","get_col_info"});

/*
result = All.FindByShortName("wpdb").GetMembersOfTarget().FindByShortName("get_results") + 
	All.FindByShortName("wpdb").GetMembersOfTarget().FindByShortName("get_var") + 
	All.FindByShortName("wpdb").GetMembersOfTarget().FindByShortName("get_row") + 
	All.FindByShortName("wpdb").GetMembersOfTarget().FindByShortName("get_col") +
	All.FindByShortName("wpdb").GetMembersOfTarget().FindByShortName("query") +
	All.FindByShortName("wpdb").GetMembersOfTarget().FindByShortName("get_col_info");
*/