CxList methods = Find_Methods();
CxList redirect = methods.FindByMemberAccess("*Response.Redirect");
redirect.Add(methods.FindByMemberAccess("*Server.Transfer")); // ASP.NET

CxList rand = All.FindByMemberAccess("Random.Next*", false);
rand.Add(All.GetParameters(methods.FindByMemberAccess("*RandomNumberGenerator.Get*", false), 0)); //.GetBytes and .GetNonZeroBytes
rand.Add(All.GetParameters(methods.FindByMemberAccess("*RNGCryptoServiceProvider.Get*", false), 0));
rand.Add(All.FindByMemberAccess("*Guid.NewGuid*", false));
redirect -= redirect.DataInfluencedBy(rand); 

CxList cs = All.FindByFileName("*.cs");
CxList ifStmt = cs.FindByType(typeof(IfStmt));

CxList notPostBack = All.FindByMemberAccess("Page.IsPostBack").GetFathers().FindByShortName("Not");
CxList ifIsNotPostBack = notPostBack.GetAncOfType(typeof(IfStmt));
ifStmt -= ifIsNotPostBack;

CxList caseStmt = cs.FindByType(typeof(Case));

result = redirect.GetByAncs(ifStmt).GetAncOfType(typeof(IfStmt)) + 
	redirect.GetByAncs(caseStmt).GetAncOfType(typeof(Case));