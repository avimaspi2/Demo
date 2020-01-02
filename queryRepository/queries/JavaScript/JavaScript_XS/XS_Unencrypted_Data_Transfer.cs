/*This query will look for any .xsaccess file in given project that has force_ssl set to "true".
In case such configuration is not found, our .xsapp file will be marked as vulnerable*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	CxList xsaccess = XSAll.FindByFileName("*.xsaccess");
	CxList tr = xsaccess.FindByShortName("true", false);
	CxList found = xsaccess.FindByShortName("force_ssl", false).GetByAncs(tr.GetFathers());

	if(found.Count == 0)
	{
		result.Add(XSAll.FindByShortName("CxJSNS*").FindByFileName("*.xsapp"));
	}
}