CxList header_inputs =	
	All.FindByMemberAccess("request.getHeaders") +
	All.FindByMemberAccess("request.getHeader") +
	All.FindByMemberAccess("request.getHeaderNames") +
	All.FindByMemberAccess("request.getContentType") +
	All.FindByMemberAccess("request.getCharacterEncoding") +
	All.FindByMemberAccess("request.getPathInfo");

result = header_inputs;