CxList dbOpen = 
	
	Find_Member_With_Target("ADODB.record", "Open") +
	Find_Member_With_Target("ADODB.recordset", "Open") +
	Find_Member_With_Target("ADODB.recordset", "Update") +
	Find_Member_With_Target("ADODB.recordset", "UpdateBatch");

CxList dbExec = 
	Find_Member_With_Target("ADODB.connection", "Execute") + 
	Find_Member_With_Target("ADODB.command", "Execute");

result = dbExec + dbOpen.GetTargetOfMembers();