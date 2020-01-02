//Finds redirects for Cross_Site_History_Manipulation

CxList methods = Find_Methods();
CxList redirect = methods.FindByMemberAccess("HttpServletResponse.sendRedirect");
redirect.Add(methods.FindByMemberAccess("ServletResponseWrapper.sendRedirect"));
redirect.Add(methods.FindByMemberAccess("HttpServletResponseWrapper.sendRedirect"));
redirect.Add(methods.FindByMemberAccess("RequestDispatcher.*")); // .forward , .include
redirect.Add(methods.FindByMemberAccess("HTTPUtilities.safeSendRedirect")); // ESAPI

result = redirect;