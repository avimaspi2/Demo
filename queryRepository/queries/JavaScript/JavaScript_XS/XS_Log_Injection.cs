/*This query will look for flow from inputs to trace mehods.*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	CxList trace = XSAll.FindByShortName("$").GetMembersOfTarget().FindByShortName("trace");
	trace = trace.GetMembersOfTarget();
	List<string> names = new List<string>(new string[]{"debug","error","fatal","warning","info"});
	CxList message = (Find_Methods() * XSAll).FindByShortNames(names);

	CxList inputs = XS_Find_Interactive_Inputs();

	result = inputs.DataInfluencingOn(message).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);	
}