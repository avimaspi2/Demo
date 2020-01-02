/// <summary>
/// Finds WebViews on PhoneGap
/// </summary>
if(PhoneGap_Found().Count > 0){
	CxList methods = Find_Methods();
	result = methods.FindByMemberAccess("window.open");
		
	CxList inAppBrowser = All.FindByName("cordova.InAppBrowser");
	List<string> openMethod = new List<string> {"open"};
	result.Add(inAppBrowser.GetMembersOfTarget().FindByShortNames(openMethod));
	result.Add(methods
		.FindByShortNames(openMethod)
		.DataInfluencedBy(inAppBrowser)
		.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));
}