/*
	This query finds all outputs that came from user input sources 
	that might expose a threat when not correctly sanitized 
*/
CxList inputs = Find_Interactive_Inputs();
inputs.Add(Find_Web_Inputs());
CxList outputs = Find_XSS_Outputs();
CxList sanitize = Find_XSS_Sanitize();

result.Add(All.FindXSS(inputs, outputs, sanitize));