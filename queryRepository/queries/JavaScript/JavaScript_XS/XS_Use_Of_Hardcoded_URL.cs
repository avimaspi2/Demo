// The query looks for the usage of *.sap.com or *wdf.sap.corp in code.

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	List<string> fileMaskList = new List<string>() {
		"*.xsaccess",
		"*.xsjslib",
		"*.xsapp",
		"*.xsjs",
	};
	result = XSAll.FindByRegexExt(@"(https?:)?(?://|\\\\)(?:[\w]+\.)*[\w]+\.sap\.com", fileMaskList);
	result.Add(XSAll.FindByRegexExt(@"(https?:)?(?://|\\\\)(?:[\w]+\.)*[\w]+\.wdf\.sap\.corp", fileMaskList));
}