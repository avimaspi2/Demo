CxList db = 
	Find_Member_With_Target("ADODB.connection", "Execute") +
	Find_Member_With_Type("Connection", "Execute") +
	Find_Member_With_Type("Recordset", "Update") +
	Find_Member_With_Type("Recordset", "UpdateBatch");
	

CxList dbExec = 
	Find_Member_With_Target("ADODB.command", "Execute") +
	Find_Member_With_Type("Command", "Execute");


CxList dbOpen = 
	Find_Member_With_Target("ADODB.record", "Open") +
	Find_Member_With_Target("ADODB.recordset", "Open") + 
	Find_Member_With_Type("Record", "Open") +
	Find_Member_With_Target("ADODB.recordset", "Update") +
	Find_Member_With_Target("ADODB.recordset", "UpdateBatch") + 
	Find_Member_With_Type("Recordset", "Open"); 

CxList DbGeneral = 
	All.FindByShortName("OpenRecordset", false) + 
	Find_Member_With_Type("Database", "Execute") + 
	All.FindByMemberAccess("object.open");


result = db + dbOpen.GetTargetOfMembers() + dbExec + DbGeneral;