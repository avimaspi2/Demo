List<String> strSafeServerParamsStrings = new List<String> {
		// Server controlled
		"GATEWAY_INTERFACE","DOCUMENT_ROOT", "SERVER_ADDR", "SERVER_SOFTWARE", "SERVER_ADMIN", "SERVER_SIGNATURE",
		// Partly server controlled
		"HTTPS","REQUEST_TIME", "REMOTE_ADDR", "REMOTE_HOST", "REMOTE_PORT", "SCRIPT_FILENAME",
		"SCRIPT_NAME","SERVER_PROTOCOL","SERVER_NAME","SERVER_PORT",
		//The fields below can't cause to vulnerability
		"REMOTE_ADDR", "REQUEST_TIME_FLOAT", "REDIRECT_REMOTE_USER"};
CxList methods = Find_Methods();
CxList fi = methods.FindByShortName("filter_input");

CxList iamSanitizer = All.FindByShortNames(strSafeServerParamsStrings).GetParameters(fi, 1);
result.Add(fi.FindByParameters(iamSanitizer));