if(cxScan.IsFrameworkActive("SAPUI")) 
{
	string regex = @"[IDC][0-9]{6,7}";
	string fileMask = "*.js";

	result = All.FindByRegexExt(regex, fileMask, true, CxList.CxRegexOptions.SearchOnlyInComments);
}