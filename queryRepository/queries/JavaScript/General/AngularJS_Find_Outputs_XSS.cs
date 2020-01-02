// Find all CxExcapedOutputs
if(cxScan.IsFrameworkActive("AngularJS"))
{
	CxList methods = Find_Methods();
	result.Add(methods.FindByShortName("CxEscapedOutput"));
}