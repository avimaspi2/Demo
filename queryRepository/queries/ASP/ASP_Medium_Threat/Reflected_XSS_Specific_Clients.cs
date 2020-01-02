CxList inputs = Find_Interactive_Inputs();
CxList outputs = Find_Interactive_Outputs()-Find_XSS_Outputs();

CxList sanitized = Find_XSS_Sanitize();

result = All.FindXSS(inputs, outputs, sanitized);