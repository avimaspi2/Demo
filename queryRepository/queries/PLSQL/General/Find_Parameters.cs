// All parametrized queries
CxList methods = Find_Methods();

CxList bind = 
	All.FindByMemberAccess("DBMS_SQL.BIND_ARRAY", false) +
	All.FindByMemberAccess("DBMS_SQL.BIND_VARIABLE", false);

CxList prepare_bind = 
	All.FindByMemberAccess("OWA_UTIL.BIND_VARIABLES", false);

//all the parameters bind variables besides the first parameter which creates the SQL statement
prepare_bind = All.GetParameters(prepare_bind) - All.GetParameters(prepare_bind, 0);

CxList using_execute = 		
	methods.FindByShortName("EXECUTE_IMMEDIATE", false)
		.GetMembersOfTarget().FindByShortName("INTO", false)
		.GetMembersOfTarget().FindByShortName("USING", false);

CxList using_open_cursor = 		
	methods.FindByShortName("OPEN", false) - All.FindByMemberAccess("*.OPEN", false);
using_open_cursor = 
	using_open_cursor.GetMembersOfTarget().FindByShortName("FOR", false)
	.GetMembersOfTarget().FindByShortName("USING", false);

result = bind + prepare_bind + using_execute + using_open_cursor;