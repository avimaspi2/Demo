/********************************************************************************************************************
There are 5 ways to redirect: 1)Response.sendRedirect
                              2)Response.setStatus(redirect status) and than Response.setHeader("Location", param)
                              3)Response.seeOther() or Response.temporaryRedirect() and then build()
							  4)Redirect in Apache using a BaseHttpResponse
							  5)Redirect using Scala/Java Play Framework
*******************************************************************************************************************/
CxList methods = Find_Methods();

//---(1) Using response.sendRedirect ---

//Find all the Responses
CxList Responses = All.FindByType("HttpServletResponse") + All.FindByShortName("*response", false);

//Find all the response.sendRedirect
CxList sendRedirect = All.FindByMemberAccess("HttpServletResponse.sendRedirect") 
					+ Responses.FindByMemberAccess("sendRedirect");

result.Add(sendRedirect);

//---(2) Using response.setStatus and then Response.setHeader ---

//Find the Response Members
CxList responseMembers = Responses.GetMembersOfTarget();

//Find status for redirection: 301 (Moved Permanently), 303 (seeOther), 302 and 307 (Temporary Redirect)
CxList statusToRedirect = All.FindByType(typeof(IntegerLiteral)).FindByShortNames(new List<string> {"301", "302", "303", "307"});

//Find more ways to represent httpStatus for redirection
List<string> httpStatus = new List<string>{"SC_MOVED_PERMANENTLY", "SC_MOVED_TEMPORARILY", "SC_TEMPORARY_REDIRECT", "SC_SEE_OTHER"};
statusToRedirect.Add(responseMembers.FindByShortNames(httpStatus));
statusToRedirect.Add(All.FindByShortName("HttpStatus").GetMembersOfTarget().FindByShortNames(httpStatus));

//Find all setStatus methods
CxList setStatus = responseMembers.FindByType(typeof(MethodInvokeExpr)).FindByShortName("setStatus");

//Find the setStatus methdos with the relevane redirect status
setStatus = Find_By_Parameter_Position(setStatus, 0, statusToRedirect);

//Find out if seHheader method was called after the relevant setstatus
CxList setHeader = methods.FindByShortName("setHeader");
setHeader = setStatus.DataInfluencingOn(setHeader).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

//Find out if the purpose of setHeader is to set the location
CxList locationParam = Find_Strings().FindByShortName("Location");
result.Add(Find_By_Parameter_Position(setHeader, 0, locationParam));

//---(3) Using a Response.ResponseBuilder ---

//Find call to ResponseBuilder.build() method
CxList responseBuilder = All.FindByMemberAccess("ResponseBuilder.build");

//Find call to Response.seeOther() or Response.temporaryRedirect() method
CxList redirectMethods = methods.FindByShortNames(new List<string> {"seeOther", "temporaryRedirect"});

//Check existance of flow between these two method calls (last one is redirect)
result.Add(redirectMethods.InfluencedBy(responseBuilder).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

//---(4) (in APACHE) Using a BaseHttpresponse ---

//Adding the BasicHttpResponse members to the response members, 
CxList BasicHttpResponse = All.FindByShortName("BasicHttpResponse");
responseMembers.Add(BasicHttpResponse.GetMembersOfTarget());

//Find the constructors of basicHttpResponse with redirect status codes as second parameter
CxList methodsToRedirect = Find_By_Parameter_Position(BasicHttpResponse, 1, statusToRedirect);

//Find all setStatusCode methods with the relevant redirect status codes as first parameter
CxList setStatusCode = responseMembers.FindByType(typeof(MethodInvokeExpr)).FindByShortName("setStatusCode");
setStatusCode = Find_By_Parameter_Position(setStatusCode, 0, statusToRedirect);

//Find all setStatusLine methods with the relevant redirect status codes as second parameter
CxList setStatusLine = responseMembers.FindByType(typeof(MethodInvokeExpr)).FindByShortName("setStatusLine");
setStatusLine = Find_By_Parameter_Position(setStatusLine, 1, statusToRedirect);

methodsToRedirect.Add(setStatusCode);
methodsToRedirect.Add(setStatusLine);

//Find out if addHeader method was called after the relevant methods(setstatusLine, setStatusCode, the constructor basicHttpResponse)
CxList addHeader = responseMembers.FindByShortName("addHeader");
addHeader = methodsToRedirect.DataInfluencingOn(addHeader).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

//Find out if the purpose of addHeader is to set the location
result.Add(locationParam.DataInfluencingOn(addHeader).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

//---(5) Using Scala/Java Play framework ---

//Find call to Redirect method (assumed to be from play framework)
CxList playRedirects = methods.FindByShortName("Redirect");
result.Add(playRedirects);