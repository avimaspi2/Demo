CxList escape = 
	All.FindByMemberAccess("htf.escape_url", false) + 
	All.FindByMemberAccess("htf.escape_sc", false) + 
	All.FindByMemberAccess("htp.escape_url", false) +
	All.FindByMemberAccess("htp.escape_sc", false);
result = escape;