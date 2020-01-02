/// <summary>
/// Finds Event handlers on PhoneGap InAppBrowsers
/// </summary>
CxList browsers = PhoneGap_Find_InAppBrowser();

	// find the references of browsers
CxList bFathers = browsers.GetFathers();
CxList candidates = bFathers.FindByType(typeof(Declarator));
candidates.Add(Find_Assign_Lefts().FindByFathers(bFathers));


result = All.FindAllReferences(candidates)
	.GetMembersOfTarget().FindByShortName("addEventListener");