CxList methods = Find_Methods();
CxList redirect = methods.FindByMemberAccess("*Response.Redirect", false);
redirect.Add(methods.FindByMemberAccess("*Server.Transfer", false)); // ASP.NET

CxList rand = All.FindByMemberAccess("Random.Next*", false);
rand.Add(All.GetParameters(methods.FindByMemberAccess("*RandomNumberGenerator.Get*", false), 0)); //.GetBytes and .GetNonZeroBytes
rand.Add(All.GetParameters(methods.FindByMemberAccess("*RNGCryptoServiceProvider.Get*", false), 0));
redirect -= redirect.DataInfluencedBy(rand); 

CxList vb = All.FindByFileName("*.vb");
CxList ifStmt = vb.FindByType(typeof(IfStmt));
CxList caseStmt = vb.FindByType(typeof(Case));

result = redirect.GetByAncs(ifStmt).GetAncOfType(typeof(IfStmt)) + 
	redirect.GetByAncs(caseStmt).GetAncOfType(typeof(Case));