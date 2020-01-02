//  By default there should be no reason for direct XHRs in Fiori apps.
if(cxScan.IsFrameworkActive("SAPUI") && Find_SAP_Library().Count > 0)
{
	result = Find_XmlHttp_Open();
}