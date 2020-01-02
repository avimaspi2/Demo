result = 
	All.FindByMemberAccess("response.write") +
	All.FindByName("msgbox") +
	All.FindByMemberAccess("wscript.echo") +
	Find_Member_With_Target("wscript.stdout", "write") +
	Find_Member_With_Target("wscript.stdout", "writeln") +
	All.FindByMemberAccess("stdout.write") +
	All.FindByMemberAccess("stdout.writeline") +
	Find_Outputs_CodeInjection() +
	Find_Outputs_Redirection() +
	Find_Outputs_XSRF() +
	Find_Outputs_XSS();