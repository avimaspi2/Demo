CxList isSecure = general.Find_Device_Lock_Verifications();

if(isSecure.Count == 0){
	CxList methodDecls = Find_MethodDecls();
	methodDecls -= methodDecls.FindByFileName("*CxPlugin*");
	CxList mainMethods = methodDecls.FindByShortName("main");
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