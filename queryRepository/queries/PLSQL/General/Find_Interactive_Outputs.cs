CxList tcp_outputs = 
	All.FindByMemberAccess("UTL_TCP.WRITE_LINE", false) + 
	All.FindByMemberAccess("UTL_TCP.WRITE_RAW", false) + 
	All.FindByMemberAccess("UTL_TCP.WRITE_TEXT", false);

CxList smtp_outputs = 
	All.FindByMemberAccess("UTL_SMTP.WRITE_DATA", false) + 
	All.FindByMemberAccess("UTL_SMTP.WRITE_RAW_DATA", false);

CxList mail_outputs = 
	All.FindByMemberAccess("UTL_MAIL.SEND", false) + 
	All.FindByMemberAccess("UTL_MAIL.SEND_ATTACH_RAW", false) + 
	All.FindByMemberAccess("UTL_MAIL.SEND_ATTACH_VARCHAR2", false) + 
	All.FindByMemberAccess("HTMLDB_MAIL.SEND", false);
	
result = 
	mail_outputs + 
	All.GetParameters((smtp_outputs + tcp_outputs), 1) + 
	Find_XSS_Outputs() + Find_Web_Outputs();