CxList header_outputs = 
	All.FindByMemberAccess("OWA_COOKIE.SEND", false) + 
	All.FindByMemberAccess("OWA_UTIL.MIME_HEADER", false) + 
	All.FindByMemberAccess("UTL_HTTP.SET_HEADER", false) + 
	All.FindByMemberAccess("UTL_HTTP.ADD_COOKIES", false);

result = header_outputs;