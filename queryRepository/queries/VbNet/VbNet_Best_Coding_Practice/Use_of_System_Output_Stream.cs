if(All.isWebApplication)
{
	CxList methods = Find_Methods();
	result = methods.FindByName("*Out.Write*", false);
	result.Add(methods.FindByName("*Error.Write*", false));
}