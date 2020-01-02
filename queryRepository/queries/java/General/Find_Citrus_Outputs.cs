/* Citrus Webx Outputs */
if (Find_Citrus_Framework().Count > 0)
{
	CxList calls = Find_Methods(); 
	calls.Add(Find_MemberAccess());	
	
	CxList citrusOutputs = calls.FindByMemberAccess("Context.put");

	CxList httpMethods = calls.FindByMemberAccess("Navigator.*");
	httpMethods -= httpMethods.FindByShortName("forwardTo");
	citrusOutputs.Add(httpMethods);

	result = citrusOutputs;
}