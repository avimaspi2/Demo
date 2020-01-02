CxList decode = All.FindByName("*.htmlDecode") + 
	All.FindByMemberAccess("HtmlDecoder.decode") + 
	All.FindByName("*HtmlDecoder.decode") + 
	All.FindByMemberAccess("ServletResponse.decode*");
CxList sanitize = Find_XSS_Sanitize() + Find_DB_In();
CxList output = Find_Interactive_Outputs();

result = output.InfluencedByAndNotSanitized(decode, sanitize);