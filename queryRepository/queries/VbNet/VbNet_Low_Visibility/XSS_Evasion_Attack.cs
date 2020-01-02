if(All.isWebApplication)
{
	CxList decode = All.FindByName("*Server.HtmlDecode", false);
	CxList sanitize = Find_XSS_Sanitize();
	CxList output = Find_Interactive_Outputs();

	result = output.InfluencedByAndNotSanitized(decode, sanitize);
}