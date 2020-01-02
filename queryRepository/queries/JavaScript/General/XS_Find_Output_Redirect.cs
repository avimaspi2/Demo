/*This query will look for redirect (using headers.set location*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	CxList outputsSet = XSAll.FindByMemberAccess("headers.set");
	CxList webSet = outputsSet.GetTargetOfMembers().FindByShortName("headers").GetMembersOfTarget();
	CxList location = XSAll.FindByShortName("location", false).FindByType(typeof(StringLiteral));
	CxList firstParameter = XSAll.GetParameters(webSet, 0);
		
	CxList redirect = (firstParameter * location);
	redirect.Add(firstParameter.DataInfluencedBy(location));
		
	CxList setRedirect = webSet.FindByParameters(redirect);
	result.Add(XSAll.GetParameters(setRedirect, 1));
}