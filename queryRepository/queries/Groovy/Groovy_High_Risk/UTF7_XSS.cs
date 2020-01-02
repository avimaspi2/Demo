CxList UTF7 = Find_Strings().FindByName("UTF-7");
CxList response = 
	All.FindByName("*Response.setCharacterEncoding") + 
	All.FindByName("*response.setCharacterEncoding") + 
	All.FindByMemberAccess("HttpServletResponse.setCharacterEncoding");

UTF7 = response.DataInfluencedBy(UTF7);

if (UTF7.Count > 0)
{
	CxList outputs = Find_XSS_Outputs();
	CxList inputs = Find_Interactive_Inputs();
	result = outputs.DataInfluencedBy(inputs);
}