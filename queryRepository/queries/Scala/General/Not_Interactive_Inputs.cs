result = All.NewCxList();
CxList servletRequest = All.FindByMemberAccess("ServletRequest.*");
result.Add(servletRequest.FindByMemberAccess("ServletRequest.getSession"));
result.Add(servletRequest.FindByMemberAccess("ServletRequest.getMethod"));
result.Add(servletRequest.FindByMemberAccess("ServletRequest.getAuthType"));
result.Add(servletRequest.FindByMemberAccess("ServletRequest.getIntHeader"));
result.Add(servletRequest.FindByMemberAccess("ServletRequest.getUserPrincipal"));
result.Add(All.FindByMemberAccess("Authenticator.getCurrentUser")); // ESAPI
result.Add(All.FindByMemberAccess("HTTPUtilties.isSecureChannel")); // ESAPI
result.Add(servletRequest.FindByMemberAccess("ServletRequest.getAttribute")); 

result.Add(
	All.FindByShortNames(new List<string> {
		"getAttribute",	
		"getProtocol",
		"getRemoteAddr",
		"getAttribute",
		"getRemoteHost",
		"getScheme",
		"getServerName",
		"getScheme",
		"getServerPort",
		"isSecure",
		"getContextPath",
		"getUserPrincipal"}));