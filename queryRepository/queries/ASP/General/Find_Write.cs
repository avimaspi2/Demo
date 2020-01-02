// $ASP

CxList write =
	Find_Member_With_Target_level2("Scripting.FileSystemObject", "OpenTextFile", "Write*") +
	Find_Member_With_Target_level2("Scripting.FileSystemObject", "CreateTextFile", "Write*") +

	Find_Member_With_Target("ADODB.Stream", "Write*")
	;
		
result = write + Find_Log_Outputs();