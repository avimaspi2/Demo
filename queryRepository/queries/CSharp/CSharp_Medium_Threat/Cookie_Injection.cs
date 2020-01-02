////////////////////////////////////////////////////////////////////////////
// Query: Cookie_Injection
// Purpose: Find unsanitized inputs that go into cookies
//    UPDATE (2014.10.16) - only cookies influenced by evil inputs 
//    and written to response (Cookies.Add) are reported!
////////////////////////////////////////////////////////////////////////////

CxList inputs = Find_Interactive_Inputs();
CxList sanitize = Find_XSS_Sanitize();
sanitize.Add(Find_Encrypt());
sanitize.Add(All.FindByMemberAccess("Convert.ToBase64String"));

CxList http_cookies = All.FindByType("HttpCookie");
CxList add_methods = All.FindByMemberAccess("Cookies.Add");

CxList cookies_in_response = 
	All.FindByName("Response.Cookies_*") +
	http_cookies.InfluencingOn(add_methods);

result = cookies_in_response.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);