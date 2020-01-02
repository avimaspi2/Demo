CxList header_inputs = All.FindByName("Page.Request.Headers", false);
header_inputs.Add(All.FindByMemberAccess("Request.Headers", false));
header_inputs.Add(All.FindByMemberAccess("Request.Headers", false).GetMembersOfTarget());

CxList sanitize=Find_XSS_Sanitize()
				-Find_XSS_Replace()-Find_HTML_Encode() ; 

CxList inputs = Find_Interactive_Inputs()
				-header_inputs;

CxList outputs = Find_Header_Outputs();

result = outputs.InfluencedByAndNotSanitized(inputs,sanitize);