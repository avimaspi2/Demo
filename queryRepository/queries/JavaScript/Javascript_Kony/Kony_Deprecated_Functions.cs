/*
	This query finds calls to KONY deprecated functions
*/

if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList invokes = Find_Methods() * Kony_All();
	
	result = invokes.FindByName("kony.net.invokeServiceAsync");
}