CxList tcp_get = 
	All.FindByMemberAccess("UTL_TCP.GET_LINE", false) + 
	All.FindByMemberAccess("UTL_TCP.GET_LINE_NCHAR", false) + 
	All.FindByMemberAccess("UTL_TCP.GET_RAW", false) + 
	All.FindByMemberAccess("UTL_TCP.GET_TEXT", false) + 
	All.FindByMemberAccess("UTL_TCP.GET_TEXT_NCHAR", false);

CxList tcp_read = 
	All.FindByMemberAccess("UTL_TCP.READ_LINE", false) + 
	All.FindByMemberAccess("UTL_TCP.READ_RAW", false) + 
	All.FindByMemberAccess("UTL_TCP.READ_TEXT", false);

CxList http_read = 
	All.FindByMemberAccess("UTL_HTTP.READ_LINE", false) + 
	All.FindByMemberAccess("UTL_HTTP.READ_RAW", false) + 
	All.FindByMemberAccess("UTL_HTTP.READ_TEXT", false);

CxList http_util = 
	All.GetParameters(All.FindByMemberAccess("UTL_HTTP.GET_HEADER", false), 3) + 
	All.GetParameters(All.FindByMemberAccess("UTL_HTTP.GET_HEADER_BY_NAME", false), 2) + 
	All.GetParameters(All.FindByMemberAccess("UTL_HTTP.GET_COOKIES", false)); 

CxList http_request = 
	All.FindByMemberAccess("UTL_HTTP.REQUEST", false) + 
	All.FindByMemberAccess("UTL_HTTP.REQUEST_PIECES", false);

CxList OWA_COOKIE_read =
	All.FindByMemberAccess("OWA_COOKIE.GET", false) +
	All.FindByMemberAccess("OWA_COOKIE.GET_ALL", false);

CxList OWA_UTIL_read = 
	All.FindByMemberAccess("OWA_UTIL.GET_CGI_ENV", false);


CxList APEX_array_inputs = 
	All.FindByMemberAccess("HTMLDB_APPLICATION.G_F*", false) + //global arrays which contain input fields values
	All.FindByMemberAccess("HTMLDB_APPLICATION.G_REQUEST", false);

APEX_array_inputs -= 
	All.FindByMemberAccess("HTMLDB_APPLICATION.G_FLOW_ID", false) + 
	All.FindByMemberAccess("HTMLDB_APPLICATION.G_FLOW_OWNER", false) + 
	All.FindByMemberAccess("HTMLDB_APPLICATION.G_FLOW_STEP_ID", false);

result = 
	tcp_get + 
	All.GetParameters(tcp_read + http_read, 1) + 
	OWA_COOKIE_read + 
	OWA_UTIL_read + 
	http_request + 
	APEX_array_inputs;