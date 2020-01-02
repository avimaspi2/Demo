/* This query will look for flow from db/storage out to outputs */
if(cxScan.IsFrameworkActive("XSJS"))
{	
	CxList storageOut = XS_Find_DB_Storage_Out();
	CxList outputs = XS_Find_Interactive_Outputs();
	outputs.Add(XS_Find_Mail_Outputs());
	result = outputs.InfluencedByAndNotSanitized(storageOut, XS_Find_Sanitize_XSS());
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}