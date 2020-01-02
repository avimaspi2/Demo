result = 
	Find_Member_With_Target("ADODB.Connection", "ConnectionString") + 
	Find_Member_With_Target("ADODB.Connection", "Open") + 
	Find_Member_With_Target("ADODB.Command", "ActiveConnection") + //can get a string as a parameter
	Find_Member_With_Target("ADODB.Record", "ActiveConnection") + 
	Find_Member_With_Target("ADODB.Recordset", "ActiveConnection");