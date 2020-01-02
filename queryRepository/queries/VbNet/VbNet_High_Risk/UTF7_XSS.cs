if(All.isWebApplication)
{
	CxList UTF7 = Find_Strings().FindByName("UTF-7");
	CxList response = All.FindByName("Response.Charset");

	UTF7 = response.DataInfluencedBy(UTF7);

	if(UTF7.Count > 0)
	{
		result = Find_XSS_Outputs().DataInfluencedBy(Find_Interactive_Inputs());
	}
}