/*This query will make sure that there is a sufficient protection against ClickJacking.
It will first look whether we have insufficient protection on the client side javascript, and it will also look
 whether we don't have ANY usage of X-Frame-Options attribute in any xsaccess file in the project.
If one of the above is true, we will report the .xsapp file to be marked as potentially vulnerable */

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	CxList client = Find_Insufficient_ClickJacking_Protection_OnClient();
	CxList xFrameOptions = XSAll.FindByShortName("X-Frame-Options", false).FindByFileName("*.xsaccess");
	
	if(client.Count > 0 || xFrameOptions.Count == 0)
	{
		result.Add(XSAll.FindByShortName("CxJSNS*").FindByFileName("*.xsapp"));
	}

	CxList allRelevantToXFrameOptions = XSAll.GetByAncs(xFrameOptions.GetAncOfType(typeof(NamespaceDecl)));

	CxList val = All.FindByShortName("value", false) * allRelevantToXFrameOptions;
	
	//If we do have a usage of X-Frame-Options attribute and it is not set to "DENY" we will report the .xsaccess file as
	// vulnerable
	CxList relevantValue = allRelevantToXFrameOptions.DataInfluencingOn(val);
	
	foreach(CxList customHeaderValue in val)
	{
		CxList rv = relevantValue.DataInfluencingOn(customHeaderValue);
		rv -= rv.FindByShortName("DENY", false);
		if(rv.Count > 0)
		{
			result.Add(customHeaderValue);
		}
	}
}