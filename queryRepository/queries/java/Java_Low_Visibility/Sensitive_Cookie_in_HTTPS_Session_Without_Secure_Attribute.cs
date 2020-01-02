// Find the setSecured(true)
CxList setSecure = All.FindByMemberAccess("Cookie.setSecure");
CxList securedParams = All.FindByShortName("true");
CxList secured = setSecure.FindByParameters(securedParams);

// Find the added cookies 
CxList addCookie =All.FindByMemberAccess("response.addCookie");
addCookie.Add(All.FindByName("*response.addCookie"));
addCookie.Add(All.FindByName("*Response.addCookie"));

CxList cookies = All.GetParameters(addCookie).FindByTypes(new String[]{"*.Cookie","Cookie"});

// Return the added cookies that are not secured
result = cookies - cookies.DataInfluencedBy(secured);