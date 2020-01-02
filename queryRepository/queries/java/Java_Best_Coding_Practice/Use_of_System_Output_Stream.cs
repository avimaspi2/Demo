if(All.isWebApplication)
{
	CxList methods = Find_Methods();
	result = methods.FindByMemberAccess("out.print*");
	result.Add(methods.FindByMemberAccess("err.print*"));
}