CxList methods = Find_Methods();

CxList db = All.FindByShortName("*db").GetMembersOfTarget();

result = db.FindByShortNames(new List<string> {"query", "get_results", "get_var","get_row","get_col","get_col_info"});
/*
result = db.FindByShortName("query") + 
	db.FindByShortName("get_results") + 
	db.FindByShortName("get_var") + 
	db.FindByShortName("get_row") + 
	db.FindByShortName("get_col") +
	db.FindByShortName("get_col_info"); 
*/
/*
result = methods.FindByShortName("query") + 
	methods.FindByShortName("get_results") + 
	methods.FindByShortName("get_var") + 
	methods.FindByShortName("get_row") + 
	methods.FindByShortName("get_col") +
	methods.FindByShortName("get_col_info"); 
*/