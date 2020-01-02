CxList redirect = 
	All.FindByMemberAccess("OWA_UTIL.REDIRECT_URL", false) + 
	All.GetParameters(All.FindByMemberAccess("HTMLDB_CUSTOM_AUTH.LOGOUT", false),2);
	
CxList inputs = Find_Interactive_Inputs();

result = redirect.DataInfluencedBy(inputs);