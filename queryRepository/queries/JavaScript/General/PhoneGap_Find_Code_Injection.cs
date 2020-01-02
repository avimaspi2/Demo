/// <summary>
/// Collects the Script Executions on a PhoneGap webView
/// </summary>
// Apply only if PhoneGap code is found to avoid collisions wih regular JS
if(PhoneGap_Found().Count > 0){
	// Find the WebViews
	CxList wviews = PhoneGap_Find_InAppBrowser();
		
	// find the references of browsers
	CxList bFathers = wviews.GetFathers();
	CxList candidates = bFathers.FindByType(typeof(Declarator));
	candidates.Add(Find_Assign_Lefts().FindByFathers(bFathers));
	wviews.Add(All.FindAllReferences(candidates));
	
	// Script execution on view
	CxList scripts = wviews.GetMembersOfTarget().FindByShortName("executeScript");
	
	// only parameters are exploitable
	result.Add(All.GetParameters(scripts));
}