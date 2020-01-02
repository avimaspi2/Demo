/*
	SSRF ( Server Side Request Forgery )
	Query to find places in code where an internal call is created based in 
	the input that comes from an outside request
*/
CxList inputs = All.NewCxList();
CxList outputs = All.NewCxList();
CxList sanitizers = All.NewCxList();

inputs.Add(Find_Interactive_Inputs());
inputs.Add(Find_Web_Inputs());

outputs.Add(Find_Remote_Requests());
outputs.Add(Find_Exec_Outputs());

sanitizers.Add(Find_General_Sanitize());
sanitizers.Add(Find_DB_Sanitize() - Find_WhiteListSanitizers());
sanitizers.Add(Find_XSS_Sanitize() - Find_WhiteListSanitizers());

CxList ssrf = outputs.InfluencedByAndNotSanitized(inputs, sanitizers);
result = ssrf.ReduceFlowByPragma();