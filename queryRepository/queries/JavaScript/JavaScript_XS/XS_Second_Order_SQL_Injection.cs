/*This quey Will look for flow from any db/storage out to db_in*/

if(cxScan.IsFrameworkActive("XSJS"))
{	
	CxList storage = XS_Find_DB_Storage_Out();
	CxList dbIn = XS_Find_DB_In();
	CxList sanitize = XS_Find_Integers() + XS_Find_SQLi_Sanitize();
	result.Add(dbIn.InfluencedByAndNotSanitized(storage, sanitize));
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}