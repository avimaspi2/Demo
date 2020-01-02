if(All.isWebApplication)
{
	CxList methods = Find_Methods();
	result = methods.FindByName("*Out.Write*") + methods.FindByName("*Error.Write*");
}