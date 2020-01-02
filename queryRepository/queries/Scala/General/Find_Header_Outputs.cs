result = All.NewCxList();
result.Add(All.FindByMemberAccess("HttpServletResponse.addHeader"));
result.Add(All.FindByMemberAccess("HttpServletResponse.setHeader"));
result.Add(All.FindByMemberAccess("HttpServletResponse.setContentType"));
result.Add(All.FindByMemberAccess("HttpServletResponse.addCookie"));
result.Add(All.FindByMemberAccess("HttpServletResponse.sendRedirect"));
	
	// ESAPI
result.Add(All.FindByMemberAccess("HTTPUtilities.setContentType"));
result.Add(All.FindByMemberAccess("HTTPUtilities.safeSendRedirect"));
result.Add(All.FindByMemberAccess("HTTPUtilities.safeSetHeader"));
result.Add(All.FindByMemberAccess("HTTPUtilities.safeAddCookie"));

result.Add(All.FindByName("*response.addHeader"));
result.Add(All.FindByName("*response.setHeader"));
result.Add(All.FindByName("*response.setContentType"));
result.Add(All.FindByName("*response.addCookie"));
result.Add(All.FindByName("*response.sendRedirect"));

result.Add(All.FindByName("*Response.addHeader"));  
result.Add(All.FindByName("*Response.setHeader"));
result.Add(All.FindByName("*Response.setContentType"));
result.Add(All.FindByName("*Response.addCookie"));
result.Add(All.FindByName("*Response.sendRedirect"));

//Akka
CxList methods = Find_Methods();
result.Add(All.FindByMemberAccess("addHeader"));
result.Add(All.FindByMemberAccess("addHeaders"));

CxList httpResponse = Find_Object_Create().FindByName("HttpResponse");
httpResponse += methods.FindByName("HttpResponse");
CxList secondParams = All.GetParameters(httpResponse, 1);
secondParams -= secondParams.FindByType(typeof(AssignExpr));
secondParams -= secondParams.FindByType(typeof(Param));
result.Add(All.GetByAncs(All.GetParameters(httpResponse))
	.FindByShortName("headers").FindByAssignmentSide(CxList.AssignmentSide.Left).GetAssigner());
result.Add(secondParams);