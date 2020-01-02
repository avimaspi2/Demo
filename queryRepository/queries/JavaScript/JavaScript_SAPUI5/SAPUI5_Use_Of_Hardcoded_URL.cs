if(cxScan.IsFrameworkActive("SAPUI"))
{
	List<string> fileMaskList = new List<string>() {
			"*.properties",
			"*.js",
			"*.xml",
			"*.html",
			"*.css",
			"*.json"
			};
	result.Add(
		All.FindByRegexExt(
			@"(https?:)?(?://|\\\\)?(?:[\w]+\.)*[\w-]+((\.wdf\.sap|\.sapinternal)\.corp)|(\.sap\.com)",
			fileMaskList,
			true
		)
	);
}