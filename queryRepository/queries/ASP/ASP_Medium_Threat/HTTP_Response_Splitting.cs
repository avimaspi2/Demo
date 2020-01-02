CxList header_inputs =	
	All.FindByName("page.request.headers") +
	All.FindByMemberAccess("request.headers") +
	All.FindByMemberAccess("request.headers").GetMembersOfTarget();

CxList sanitize = Find_XSS_Sanitize()
	- Find_XSS_Replace() - Find_HTML_Encode();

CxList inputs = Find_Interactive_Inputs()
	- header_inputs;

CxList outputs = Find_Header_Outputs();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);