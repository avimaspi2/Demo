// This query will look for response redirect that is influenced by input

if(cxScan.IsFrameworkActive("XSJS"))
{	
	CxList inputs = XS_Find_Interactive_Inputs();
	CxList redirect = XS_Find_Output_Redirect();
	CxList sanitize = XS_Find_Integers();
	result = redirect.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}