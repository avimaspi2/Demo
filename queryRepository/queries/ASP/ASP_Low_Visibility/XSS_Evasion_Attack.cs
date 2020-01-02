CxList decode = All.FindByName("*server.htmldecode");
CxList sanitize = Find_XSS_Sanitize();
CxList output = Find_Interactive_Outputs();

result = output.InfluencedByAndNotSanitized(decode, sanitize);