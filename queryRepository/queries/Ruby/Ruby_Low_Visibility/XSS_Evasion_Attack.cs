CxList decode = All.FindByName("*decode*", false);
CxList sanitize = Find_XSS_Sanitize();
CxList output = Find_Interactive_Outputs();

result = output.InfluencedByAndNotSanitized(decode, sanitize);