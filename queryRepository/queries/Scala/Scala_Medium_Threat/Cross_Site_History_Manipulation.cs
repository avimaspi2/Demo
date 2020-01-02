CxList methods = Find_Methods();
CxList redirect = methods.FindByMemberAccess("HttpServletResponse.sendRedirect");
redirect.Add(methods.FindByMemberAccess("ServletResponseWrapper.sendRedirect"));
redirect.Add(methods.FindByMemberAccess("HttpServletResponseWrapper.sendRedirect"));
redirect.Add(methods.FindByMemberAccess("RequestDispatcher.*")); // .forward , .include
redirect.Add(methods.FindByMemberAccess("HTTPUtilities.safeSendRedirect")); // ESAPI

CxList rand = All.FindByMemberAccess("Random.Next*", false); // next(), nextInt...
rand.Add(All.FindByMemberAccess("Math.random", false));
rand.Add(Get_ESAPI().FindByMemberAccess("Randomizer.*")); // ESAPI
rand.Add(All.GetParameters(methods.FindByMemberAccess("*Random.nextBytes", false), 0)); 
// for .nextBytes, the random number is stored in a buffer passed as a parameter to the nextBytes method.
redirect -= redirect.DataInfluencedBy(rand);

CxList ifStmt = All.FindByType(typeof(IfStmt));
CxList caseStmt = All.FindByType(typeof(Case));

result = redirect.GetByAncs(ifStmt).GetAncOfType(typeof(IfStmt)) + 
	redirect.GetByAncs(caseStmt).GetAncOfType(typeof(Case));