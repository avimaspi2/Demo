result =
	All.FindByMemberAccess("ServletRequest.getSession") + 
	All.FindByMemberAccess("ServletRequest.getMethod") +
	All.FindByMemberAccess("ServletRequest.getAuthType") +
	All.FindByMemberAccess("ServletRequest.getIntHeader") +
	All.FindByMemberAccess("ServletRequest.getUserPrincipal") + 
	All.FindByMemberAccess("Authenticator.getCurrentUser") + // ESAPI
	All.FindByMemberAccess("HTTPUtilties.isSecureChannel") + // ESAPI
	All.FindByMemberAccess("ServletRequest.getAttribute") +
	All.FindByShortName("getAttribute") + 	
	All.FindByShortName("getProtocol") +
	All.FindByShortName("getRemoteAddr") +
	All.FindByShortName("getAttribute") +
	All.FindByShortName("getRemoteHost") +
	All.FindByShortName("getScheme") + 
	All.FindByShortName("getServerName") + 
	All.FindByShortName("getScheme") + 
	All.FindByShortName("getServerPort") + 
	All.FindByShortName("isSecure") + 
	All.FindByShortName("getContextPath") + 
	All.FindByShortName("getUserPrincipal");