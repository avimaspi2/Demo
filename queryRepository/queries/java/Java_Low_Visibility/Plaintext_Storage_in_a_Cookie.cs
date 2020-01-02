CxList addCookie = 	All.FindByMemberAccess("HttpServletRequest.addCookie");
addCookie.Add(All.FindByName("*response.addCookie"));  
addCookie.Add(All.FindByName("*Response.addCookie"));
addCookie.Add(All.FindByMemberAccess("HTTPUtilities.safeAddCookie")); // ESAPI

CxList cookies = All.FindByType("Cookie");
CxList cookieParam = All.GetParameters(cookies, 1).FindByType(typeof(UnknownReference));

CxList plainText = Find_Strings();

CxList sanitize = Find_General_Sanitize();

result = plainText.InfluencingOnAndNotSanitized(cookieParam, sanitize);