// $ASP*

CxList db = 
	Find_Member_With_Target("ADODB.connection", "Execute") +
	Find_Member_With_Target("ADODB.recordset", "Update") +
	Find_Member_With_Target("ADODB.recordset", "UpdateBatch");

CxList dbExec = Find_Member_With_Target("ADODB.command", "Execute");
CxList dbExecMethods = dbExec.FindByType(typeof(MethodInvokeExpr));

CxList dbExecMethodsNoParanthesis = dbExec.FindByType(typeof(MemberAccess));
CxList commandObjRef = All.FindAllReferences(dbExecMethodsNoParanthesis.GetTargetOfMembers()); 
CxList commandText = commandObjRef.GetMembersOfTarget().FindByShortName("CommandText", false);

CxList dbOpen = 
	Find_Member_With_Target("ADODB.record", "Open") +
	Find_Member_With_Target("ADODB.recordset", "Open");
CxList dbOpenMethods = dbOpen.FindByType(typeof(MethodInvokeExpr));

CxList dbOpenMethodsNoParanthesis = dbOpen.FindByType(typeof(MemberAccess));
CxList openObjRef = All.FindAllReferences(dbOpenMethodsNoParanthesis.GetTargetOfMembers()); 
CxList commandSource = openObjRef.GetMembersOfTarget().FindByShortName("Source", false);

result = db + dbExecMethods + commandText + dbOpenMethods + commandSource;