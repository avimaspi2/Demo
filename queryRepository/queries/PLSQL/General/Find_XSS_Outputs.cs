CxList http_outputs = 
	All.FindByMemberAccess("UTL_HTTP.WRITE_RAW", false) + 
	All.FindByMemberAccess("UTL_HTTP.WRITE_TEXT", false);
	
result = http_outputs + Find_Html_Outputs();