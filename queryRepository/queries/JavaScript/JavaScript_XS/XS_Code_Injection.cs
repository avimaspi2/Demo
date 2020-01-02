// This query will look for flow form XS interactive inputs to code evaluation procedures

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	// Handle the var z = new Function('a','b','a+b') type of evaluation 
	CxList functionObject = XSAll.FindByShortName("CxEval").GetAncOfType(typeof(IfStmt));
	CxList sink = XSAll.GetByAncs(functionObject);
	
	// Handle standard Typescript "eval"
	sink.Add(XSAll.FindByShortName("eval"));
	
	CxList inputs = XS_Find_Interactive_Inputs();
	CxList sanitize = XS_Find_Integers();
	result = inputs.InfluencingOnAndNotSanitized(sink, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}