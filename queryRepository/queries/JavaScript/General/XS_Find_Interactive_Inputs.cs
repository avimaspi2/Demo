/*This query will look for ineractive inputs
Interatve inputs will be considered any members of the $.request
also will be considered the $.net.http.Client getResopnse
*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	//web 
	CxList XSAll = XS_Find_All();
	CxList inputs = XSAll.FindByMemberAccess("$.request");
	inputs.Add(XSAll.FindByShortName("$").GetMembersOfTarget().FindByShortName("request"));
	CxList requestMember = inputs.GetRightmostMember();
	result.Add(requestMember);

	//client
	CxList oce = XSAll.FindByType(typeof(ObjectCreateExpr));
	CxList client = oce.FindByShortName("Client");
	CxList methods = Find_Methods() * XSAll;
	CxList getResponse = methods.FindByShortName("getResponse");
	CxList httpClientResponse = getResponse.GetTargetOfMembers().DataInfluencedBy(client);
	result.Add(httpClientResponse.GetMembersOfTarget());
}