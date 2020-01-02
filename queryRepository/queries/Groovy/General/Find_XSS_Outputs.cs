CxList sanitizers = All.GetByAncs(Find_XSS_Sanitize());
result = Find_Web_Outputs() - Find_Header_Outputs() + Find_Html_Outputs() - sanitizers;
result -= result.GetTargetOfMembers();