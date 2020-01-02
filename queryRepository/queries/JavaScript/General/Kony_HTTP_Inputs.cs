if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList konyAll = Kony_All();	

	CxList newHttpObj = konyAll.FindByName("*kony.net.HttpRequest");

	CxList HttpInstances = konyAll.FindAllReferences(newHttpObj.GetAssignee());
	CxList open = HttpInstances.GetMembersOfTarget().FindByShortName("response");

	result = konyAll.GetParameters(open, 1);
}