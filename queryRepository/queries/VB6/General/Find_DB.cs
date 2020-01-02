CxList db = Find_Member_With_Target("ADODB.connection", "Execute");
db.Add(Find_Member_With_Target("ADODB.recordset", "Update"));
db.Add(Find_Member_With_Target("ADODB.recordset", "UpdateBatch"));
db.Add(Find_Member_With_Type("Connection", "Execute"));
db.Add(Find_Member_With_Type("Recordset", "Update"));
db.Add(Find_Member_With_Type("Recordset", "UpdateBatch"));
	

CxList dbExec = 
	Find_Member_With_Target("ADODB.command", "Execute") +
	Find_Member_With_Type("Command", "Execute");

CxList dbExecMethods = dbExec.FindByType(typeof(MethodInvokeExpr));

CxList dbExecMethodsNoParanthesis = dbExec.FindByType(typeof(MemberAccess));
CxList commandObjRef = All.FindAllReferences(dbExecMethodsNoParanthesis.GetTargetOfMembers()); 
CxList commandText = commandObjRef.GetMembersOfTarget().FindByShortName("CommandText", false);

CxList dbOpen = Find_Member_With_Target("ADODB.record", "Open");
dbOpen.Add(Find_Member_With_Target("ADODB.recordset", "Open")); 
dbOpen.Add(Find_Member_With_Type("Record", "Open"));
dbOpen.Add(Find_Member_With_Type("Recordset", "Open")); 

CxList dbOpenMethods = dbOpen.FindByType(typeof(MethodInvokeExpr));

CxList dbOpenMethodsNoParanthesis = dbOpen.FindByType(typeof(MemberAccess));
CxList openObjRef = All.FindAllReferences(dbOpenMethodsNoParanthesis.GetTargetOfMembers()); 
CxList commandSource = openObjRef.GetMembersOfTarget().FindByShortName("Source", false);

CxList DbGeneral = 
	All.FindByShortName("OpenRecordset", false) + 
	Find_Member_With_Type("Database", "Execute") + 
	All.FindByMemberAccess("object.open");


result = db + dbOpenMethods + commandSource + dbExecMethods + commandText + DbGeneral;