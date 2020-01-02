/*This query will look for flow from db/store out to evaluation procedure*/
if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	// Find "CxEval" and "eval"
	CxList sink = XSAll.GetParameters((XSAll*Find_ObjectCreations()).FindByShortName("Function")) - Find_Param();
	sink.Add(XSAll.FindByShortName("eval"));
	
	CxList storage = XS_Find_DB_Storage_Out();
	CxList sanitize = XS_Find_Integers();
	result = storage.InfluencingOnAndNotSanitized(sink, sanitize);
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}