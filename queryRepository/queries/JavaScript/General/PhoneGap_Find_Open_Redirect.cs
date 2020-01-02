/// <summary>
/// Finds WebView Browser opening Urls
/// </summary>
// Enable only if the PhoneGap code is found to avoid coliisions with regular JS
if(PhoneGap_Found().Count > 0){
	CxList wviews = PhoneGap_Find_InAppBrowser();
	// exploitable url to be opened
	result = wviews;
	result.Add(All.GetParameters(wviews, 0));
}