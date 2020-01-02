CxList methods = Find_Methods();

CxList execute = 
	All.FindByMemberAccess("DBMS_UTILITY.EXEC_DDL_STATEMENT", false) + 
	methods.FindByShortName("EXECUTE_IMMEDIATE", false);
	
CxList create_dynamic = 
	All.FindByMemberAccess("DBMS_DDL.CREATE_WRAPPED", false) +
	All.FindByMemberAccess("OWA_UTIL.BIND_VARIABLES", false) + 
	All.FindByMemberAccess("OWA_UTIL.CELLSPRINT", false) + 
	All.FindByMemberAccess("OWA_UTIL.CALENDARPRINT", false);

//Only the first parameter is the SQL statement
create_dynamic = All.GetParameters(create_dynamic, 0);
	
CxList parse = All.FindByMemberAccess("DBMS_SQL.PARSE", false);
CxList parse_param = All.GetParameters(parse, 1);

//handling ref cursors which execute a dynamic query
CxList open_for_cursor = 		
	methods.FindByShortName("OPEN", false) - 
	All.FindByMemberAccess("*.OPEN", false);
open_for_cursor = open_for_cursor.GetMembersOfTarget().FindByShortName("FOR", false);

CxList APEX_query = 
	All.FindByMemberAccess("HTMLDB_ITEM.POPUP_FROM_QUERY", false) + 
	All.FindByMemberAccess("HTMLDB_ITEM.POPUPKEY_FROM_QUERY", false) + 
	All.FindByMemberAccess("HTMLDB_ITEM.SELECT_LIST_FROM_QUERY", false) + 
	All.FindByMemberAccess("HTMLDB_ITEM.SELECT_LIST_FROM_QUERY_XL", false);

APEX_query = All.GetParameters(APEX_query, 2);

result = execute + create_dynamic + parse_param + open_for_cursor + APEX_query;