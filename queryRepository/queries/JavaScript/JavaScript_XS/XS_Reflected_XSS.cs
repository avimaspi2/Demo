/* The query looks for flow from inputs to outputs */

if(cxScan.IsFrameworkActive("XSJS"))
{	
	CxList inputs = XS_Find_Interactive_Inputs();
	CxList outputs = XS_Find_Interactive_Outputs();
	outputs.Add(XS_Find_Mail_Outputs());
	
	result.Add(outputs.InfluencedByAndNotSanitized(inputs, XS_Find_Sanitize_XSS()));
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}