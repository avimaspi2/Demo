CxList redirect = 
	All.FindByMemberAccess("HttpServletResponse.sendRedirect") +
	All.FindByName("*response.sendRedirect") +
	All.FindByName("*Response.sendRedirect");
CxList inputs = Find_Interactive_Inputs();
CxList sanitize = Find_Redirect_Sanitizers();

result = redirect.InfluencedByAndNotSanitized(inputs, sanitize);