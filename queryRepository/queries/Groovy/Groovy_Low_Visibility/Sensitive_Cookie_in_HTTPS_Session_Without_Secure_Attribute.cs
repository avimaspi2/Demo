// Find the setSecured(true)
CxList setSecure = All.FindByMemberAccess("Cookie.setSecure");
CxList securedParams = All.FindByShortName("true");
CxList secured = setSecure.FindByParameters(securedParams);

// Find the added cookies 
CxList addCookie =
	All.FindByMemberAccess("response.addCookie") +
	All.FindByName("*response.addCookie") +
	All.FindByName("*Response.addCokies");
CxList cookies = All.GetParameters(addCookie).FindByType("Cookie");

// Return the added cookies that are not secured
result = cookies - cookies.DataInfluencedBy(secured);