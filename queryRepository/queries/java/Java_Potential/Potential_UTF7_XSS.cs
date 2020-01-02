CxList UTF7 = Find_Strings().FindByName("UTF-7");

CxList response = All.FindByMemberAccess("HttpServletResponse.setCharacterEncoding");
response.Add(All.FindByName("*response.setCharacterEncoding"));  
response.Add(All.FindByName("*Response.setCharacterEncoding"));

UTF7 = response.DataInfluencedBy(UTF7);

if (UTF7.Count > 0)
{
	CxList output = Find_XSS_Outputs();
	CxList input = Find_Potential_Inputs();
	result = output.DataInfluencedBy(input);
}