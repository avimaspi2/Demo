CxList connectionObjectCreate = 
	Find_Member_With_Target("ADODB.Connection", "Open") + 
	Find_Member_With_Target("ADODB.Connection", "ConnectionString");

CxList connectionObjectDef = 
	Find_Member_With_Type("Connection", "Open") + 
	Find_Member_With_Type("Connection", "ConnectionString");

result = connectionObjectCreate + connectionObjectDef;