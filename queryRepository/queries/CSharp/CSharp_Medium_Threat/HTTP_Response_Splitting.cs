CxList request_headers = All.FindByMemberAccess("Request.Headers");

CxList header_inputs = All.FindByName("Page.Request.Headers");
header_inputs.Add(request_headers);
header_inputs.Add(request_headers.GetMembersOfTarget());

CxList sanitize = Find_XSS_Sanitize()
	- Find_XSS_Replace() - Find_HTML_Encode(); 

CxList inputs = Find_Interactive_Inputs()
	- header_inputs;

CxList outputs = Find_Change_Response_Header();

// If only the FileInfo.Name is passed as parameter to Response.AppendHeader and not the input string then is not vulnerable
CxList fileInfoNames = All.FindByType("FileInfo").GetMembersOfTarget().FindByShortNames(new List<string> {"FullName","Name"});
sanitize.Add(outputs.FindByParameters(fileInfoNames));

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);