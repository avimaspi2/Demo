result = 
	All.FindByMemberAccess("HttpServletResponse.addHeader") + 
	All.FindByMemberAccess("HttpServletResponse.setHeader") + 
	All.FindByMemberAccess("HttpServletResponse.setContentType") + 
	All.FindByMemberAccess("HttpServletResponse.addCookie") +
	All.FindByMemberAccess("HttpServletResponse.sendRedirect") +
	
	// ESAPI
	All.FindByMemberAccess("HTTPUtilities.setContentType") +
	All.FindByMemberAccess("HTTPUtilities.safeSendRedirect") +
	All.FindByMemberAccess("HTTPUtilities.safeSetHeader") +
	All.FindByMemberAccess("HTTPUtilities.safeAddCookie") +

	All.FindByName("*response.addHeader") +  
	All.FindByName("*response.setHeader") +
	All.FindByName("*response.setContentType") +
	All.FindByName("*response.addCookie") +
	All.FindByName("*response.sendRedirect") +

	All.FindByName("*Response.addHeader") +  
	All.FindByName("*Response.setHeader") +
	All.FindByName("*Response.setContentType") +
	All.FindByName("*Response.addCookie") +
	All.FindByName("*Response.sendRedirect")

	;