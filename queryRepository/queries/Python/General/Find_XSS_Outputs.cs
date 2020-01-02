CxList sanitizers = All.GetByAncs(Find_XSS_Sanitize());

CxList headerOutSanitizers = Find_Header_Outputs();
headerOutSanitizers.Add(sanitizers);

result = Find_Web_Outputs() - headerOutSanitizers;
result -= result.GetTargetOfMembers();