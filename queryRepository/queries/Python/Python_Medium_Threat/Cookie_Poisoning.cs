/*This query finds inputs which goes to the cookie set-header*/
CxList cookies = Find_Cookies();
//Find_Inputs() returns Find_Cookies()
CxList inputs = Find_Inputs() - cookies;
//Find all the cookies references.
CxList allRefsCookies = All.FindAllReferences(cookies);

allRefsCookies = allRefsCookies.GetMembersOfTarget();
//Find cookie field assignment like: cookie["session"]
CxList cookieDataSet = allRefsCookies.FindByType(typeof(MemberAccess));	
//Find method load which loads the cookie from a string
CxList loadMethod = allRefsCookies.FindByShortName("load");
cookieDataSet.Add(loadMethod);

//Get right side of cookie Assignment and add it to sinks list. For cases like: Cookie.SimpleCookie(input)
CxList cookieAssignExpr = cookies.GetAncOfType(typeof(AssignExpr));
CxList rightSideCookieAssign = All.GetByAncs(cookieAssignExpr).FindByAssignmentSide(CxList.AssignmentSide.Right);
cookieDataSet.Add(rightSideCookieAssign);

//Find set_cookie invoke (django related)
CxList setCookieInvoke = cookies.FindByShortName("set_cookie");
cookieDataSet.Add(setCookieInvoke);

//find cases like: cookie["session"] = input
result = cookieDataSet.DataInfluencedBy(inputs).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);