if(All.isWebApplication)
{
	CxList decode = All.FindByName("*Server.HtmlDecode");
	CxList sanitize = Find_XSS_Sanitize();
	CxList output = Find_XSS_Outputs();

	result = output.InfluencedByAndNotSanitized(decode, sanitize);
}