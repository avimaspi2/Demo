CxList methods = Find_Methods();
CxList inputs = Find_Interactive_Inputs();
CxList firstParamMethods = methods.FindByShortNames(new List<String>()
	{ "fsockopen", "get_headers", "imap_open", "imap_mail", "mail", "mb_send_mail",
		"ldap_connect", "msession_connect", "pfsockopen", "stream_socket_client", "stream_socket_server" });

CxList firstParam = All.GetParameters(firstParamMethods, 0);

CxList secondAndThirdParamMethods = 
	methods.FindByShortNames(new List<String>() { "curl_setopt", "ftp_chmod", "ftp_put", "ftp_nb_get", "ftp_get" });

CxList secondParamMethods =
	methods.FindByShortNames(new List<String>()
	{ "curl_setopt_array", "cyrus_query", "socket_bind", "socket_connect", "socket_send", "socket_write", "ftp_exec", "ftp_delete", "ftp_nlist", "ftp_nb_put"});
secondParamMethods.Add(secondAndThirdParamMethods);
	
CxList secondParam = All.GetParameters(secondParamMethods, 1);

CxList thirdParamMethods = 
	methods.FindByShortNames(new List<String>(){ "error_log", "ftp_fget", "ftp_nb_fget" });
thirdParamMethods.Add(secondAndThirdParamMethods);
	
CxList thirdParam = All.GetParameters(thirdParamMethods, 2);

CxList fourthParamMethods = methods.FindByShortNames(new List<String>(){ "mail", "mb_send_mail" });
CxList fourthParam = All.GetParameters(fourthParamMethods, 3);

CxList allParamMethods = methods.FindByShortName("session_register");
CxList allParam = All.GetParameters(allParamMethods);

CxList tainted_params = firstParam;
tainted_params.Add(secondParam);
tainted_params.Add(thirdParam);
tainted_params.Add(fourthParam);
tainted_params.Add(allParam);

CxList sanitize = Find_Header_Injection_Sanitize();

result = tainted_params.InfluencedByAndNotSanitized(inputs, sanitize);
result.Add(tainted_params * inputs);

//header function has no way of sanitize statically, so it will always appear as result
CxList headerMethod = methods.FindByShortName("header");
CxList headerParam = All.GetParameters(headerMethod, 0);
result.Add(headerParam.InfluencedBy(inputs));
result.Add(headerParam * inputs);