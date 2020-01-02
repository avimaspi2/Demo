//the query searches  for calls to chroot without chdir in the same scope(also checks all the functions content)
//the query has 2 parts, 1 for finding all the chroot and then check if the containing scope has chdir
//2- find all the chdir and check if the containing scope has chroot
//chroot can be named fchroot, chdir can be called fchdir


//1.serach for chroot and check if have chdir in the same scope

CxList usingOfChroot = Find_Methods().FindByName("chroot");
usingOfChroot.Add(Find_Methods().FindByName("fchroot"));
CxList unsafeChroot = usingOfChroot;

//check each chroot scope to see if has chdir
foreach (CxList curChroot in usingOfChroot)
{
	CxList statementCollection = curChroot.GetAncOfType(typeof(StatementCollection));
	CxList allItemsInScope = All.GetByAncs(statementCollection);
	
	//find all methods that are called and check if they have the chdir
	CxList methodInvokedInScope = allItemsInScope.FindByType(typeof(MethodInvokeExpr));
	CxList methodInScopeDecl = All.FindAllReferences(methodInvokedInScope).FindByType(typeof(MethodDecl));
	CxList allMethodsScope = All.GetByAncs(methodInScopeDecl);
	allItemsInScope.Add(allMethodsScope);
	
	CxList scopeDir = allItemsInScope.FindByType(typeof(MethodInvokeExpr)).FindByName("chdir");
	scopeDir.Add(allItemsInScope.FindByType(typeof(MethodInvokeExpr)).FindByName("fchdir"));
	if (scopeDir.Count > 0)
	{
		unsafeChroot -= curChroot;
	}
}


//2.serach for chdir and check if have chroot in the same scope

CxList allChdir = Find_Methods().FindByName("chdir");
allChdir.Add(Find_Methods().FindByName("fchdir"));

//check each chdir scope to see if has chroot
foreach (CxList curChdir in allChdir)
{
	CxList statementCollection = curChdir.GetAncOfType(typeof(StatementCollection));
	CxList allItemsInScope = All.GetByAncs(statementCollection);
	
	//find all methods that are called and check if they have the chroot
	CxList methodInvoedInScope = allItemsInScope.FindByType(typeof(MethodInvokeExpr));
	CxList methodInScopeDecl = All.FindAllReferences(methodInvoedInScope).FindByType(typeof(MethodDecl));
	CxList allMethodsScope = All.GetByAncs(methodInScopeDecl);
	allItemsInScope.Add(allMethodsScope);
	
	CxList scopeChroot = allItemsInScope.FindByType(typeof(MethodInvokeExpr)).FindByName("chroot");
	scopeChroot.Add(allItemsInScope.FindByType(typeof(MethodInvokeExpr)).FindByName("fchroot"));
	if (scopeChroot.Count > 0)
	{
		unsafeChroot -= scopeChroot;
	}
	
}

result = unsafeChroot;