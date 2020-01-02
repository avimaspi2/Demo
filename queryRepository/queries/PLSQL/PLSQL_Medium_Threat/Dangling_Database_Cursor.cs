CxList methods = Find_Methods();
CxList identifiers = All.FindByType(typeof(UnknownReference));
identifiers.Add(All.FindByType(typeof(MemberAccess)));

//handling DBMS_SQL cursors
CxList convert_cursor = All.FindByMemberAccess("DBMS_SQL.TO_REFCURSOR", false);
convert_cursor.Add(All.FindByMemberAccess("DBMS_SQL.TO_CURSOR_NUMBER", false));

CxList open_cursor = All.FindByMemberAccess("DBMS_SQL.OPEN_CURSOR", false);

CxList init_cursor = open_cursor.FindByAssignmentSide(CxList.AssignmentSide.Right); 
init_cursor.Add(convert_cursor.FindByAssignmentSide(CxList.AssignmentSide.Right));

CxList assign = init_cursor.GetAncOfType(typeof(AssignExpr));
CxList cursors = identifiers.GetByAncs(assign).FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList close_cursor = All.FindByMemberAccess("DBMS_SQL.CLOSE_CURSOR", false);
close_cursor.Add(convert_cursor);

CxList close_cursor_param = All.GetParameters(close_cursor);
CxList close_cursor_references = cursors.FindAllReferences(close_cursor_param);

result.Add(cursors - close_cursor_references);

//handling explicit cursors & ref cursors
CxList suspected_open_cursor = methods.FindByShortName("OPEN", false) - All.FindByMemberAccess("*.OPEN",false);

//open ref cursor
CxList for_ref_cursor = 
	methods.FindByShortName("FOR", false).GetTargetOfMembers().FindByShortName("OPEN",false) - All.FindByMemberAccess("*.OPEN", false);

//close cursor & ref cursor
CxList notCloseCursor = All.FindByMemberAccess("*.CLOSE", false);
notCloseCursor.Add(All.FindByMemberAccess("CLOSE.*", false));

CxList suspected_close_cursor = methods.FindByShortName("CLOSE", false); 
suspected_close_cursor -= notCloseCursor;
	
CxList explicit_cursor = All.FindByCustomAttribute("cursor").GetAncOfType(typeof(MethodDecl));
CxList inst_explicit_cursor = All.FindAllReferences(explicit_cursor) - explicit_cursor;

CxList open_explicit_cursor = inst_explicit_cursor.GetParameters(suspected_open_cursor, 0); 
open_explicit_cursor.Add(All.GetParameters(for_ref_cursor, 0).FindByType(typeof(UnknownReference)));
	
CxList close_explicit_cursor = inst_explicit_cursor.GetParameters(suspected_close_cursor,0);

// Implicit Cursor close FOR LOOP Statement
close_explicit_cursor.Add(All.GetParameters(for_ref_cursor, 0));

CxList close_explicit_cursor_references = All.FindAllReferences(close_explicit_cursor);
result.Add(open_explicit_cursor - close_explicit_cursor_references);