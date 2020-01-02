CxList UTF7 = Find_Strings().FindByName("utf-7");
CxList response = All.FindByName("response.charset");

UTF7 = response.DataInfluencedBy(UTF7);

if(UTF7.Count > 0)
{
	result = Find_XSS_Outputs().DataInfluencedBy(Find_Interactive_Inputs());
}