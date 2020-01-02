CxList isSecure = Find_Jailbreak_verification();

if(isSecure.Count == 0){
	CxList methodDecls = Find_MethodDecls();
	CxList mainMethods = methodDecls.FindByShortName("main");
	methodDecls -= methodDecls.FindByFileName("*CxPlugin*");
	CxList methods = All.NewCxList();
	
	if(mainMethods.Count > 0)
	{
		methods.Add(mainMethods);
	}
	else
	{
		methods.Add(methodDecls);
	}
	
	foreach(CxList m in methods)
	{
		result = m;
		break; 
	}
}