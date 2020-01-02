CxList methods = Find_Methods();

CxList values = 
	All.FindByMemberAccess("DBMS_SQL.COLUMN_VALUE", false) + 
	All.FindByMemberAccess("DBMS_SQL.COLUMN_VALUE_LONG", false);

CxList longValue = All.FindByMemberAccess("DBMS_SQL.VARIABLE_VALUE", false);

CxList selectInto = All.FindByShortName("CX_SQL_STATEMENT",false)
	.GetMembersOfTarget().FindByShortName("SELECT", false)
	.GetMembersOfTarget().FindByShortName("INTO", false);

// Deal with assignment of SELECT into a for-loop variable
//  e.g:    For rec In (select object_name, object_type
//              from user_objects)
CxList forEachSons = All.FindByFathers(All.FindByType(typeof(ForEachStmt)));
CxList selectInsideForEach = forEachSons.FindByShortName("SELECT", false);


CxList execute = methods.FindByShortName("EXECUTE_IMMEDIATE", false);
CxList executeInto = execute.GetMembersOfTarget().FindByShortName("INTO",false);

CxList usingClause = execute.GetMembersOfTarget().FindByShortName("USING", false).GetMembersOfTarget();
usingClause = usingClause.FindByShortName("RETURN", false) + usingClause.FindByShortName("RETURNING", false);
CxList returnInto = usingClause.GetMembersOfTarget().FindByShortName("INTO", false);


CxList fetchInto = methods.FindByShortName("FETCH", false);
fetchInto = 
	fetchInto.GetMembersOfTarget().FindByShortName("INTO", false) + 
	fetchInto.GetMembersOfTarget().FindByShortName("BULK_COLLECT_INTO", false); 

result = 
	All.GetParameters(values, 2) + 
	All.GetParameters(longValue, 4) + 
	All.GetParameters(selectInto) + 
	All.GetParameters(selectInsideForEach) +
	All.GetParameters(executeInto) +
	All.GetParameters(returnInto) +
	All.GetParameters(fetchInto);