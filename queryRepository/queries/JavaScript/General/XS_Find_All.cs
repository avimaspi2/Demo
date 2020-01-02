/*Find XS relevant All */

if(cxScan.IsFrameworkActive("XSJS"))
{
	result = All.FindByFileName("*.xsaccess");
	result.Add(All.FindByFileName("*.xsjs"));
	result.Add(All.FindByFileName("*.xsjslib"));
	result.Add(All.FindByFileName("*.xsapp"));
}