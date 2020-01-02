// this query will look for flow from inputs to db_in

if(cxScan.IsFrameworkActive("XSJS"))
{	
	CxList inputs = XS_Find_Interactive_Inputs();
	CxList dbIn = XS_Find_DB_In();
	CxList sanitize = XS_Find_Integers();
	sanitize.Add(XS_Find_SQLi_Sanitize());
	result = dbIn.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
	result.Add(dbIn * inputs);
}