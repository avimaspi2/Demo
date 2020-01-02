if (Find_Device_Root_Verification().Count == 0)
{
	CxList methodDecls = Find_MethodDecls();
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