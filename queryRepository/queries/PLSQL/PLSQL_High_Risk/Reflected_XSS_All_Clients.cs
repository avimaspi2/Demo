CxList inputs = Find_Inputs();
CxList outputs = Find_XSS_Outputs();
CxList sanitize = Find_XSS_Sanitize();

result = All.FindXSS(inputs, outputs, sanitize);