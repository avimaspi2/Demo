CxList methods = Find_Methods();

CxList currentPage = methods.FindByMemberAccess("apexpages.currentpage");
CxList getUrl1 = methods.FindByMemberAccess("pagereference.geturl");
CxList getUrl2 = methods.FindByMemberAccess("currentpagereference.geturl")
	+ methods.FindByMemberAccess("currentpage.geturl");
CxList getParameters = methods.FindByMemberAccess("currentpage.getparameters");
CxList currentPageMembers = currentPage.GetMembersOfTarget();

CxList getUrl3 = getUrl2.GetMembersOfTarget().GetTargetOfMembers();
getUrl3 = getUrl2 - getUrl3;

CxList retUrl = methods.FindByShortName("get").FindByParameters(Find_Strings().FindByShortName("'returl'"));

result = 
	getUrl1.InfluencedByAndNotSanitized(currentPage, getParameters) +
	getUrl2 * currentPageMembers +
	getUrl3 +
	retUrl;