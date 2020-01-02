/*This query will look for all interactive outputs
it will contain all members of the $.response 
and it will contain $.net.http.Client Request object creation
*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	//web
	CxList XSAll = XS_Find_All();
	CxList outputs = XSAll.FindByMemberAccess("$.response");
	outputs.Add(XSAll.FindByShortName("$").GetMembersOfTarget().FindByShortName("response"));
	CxList responseMember = outputs.GetRightmostMember();
	result.Add(responseMember);

	//Client request as an output
	CxList oce = XSAll.FindByType(typeof(ObjectCreateExpr));
	CxList httpRequest = oce.FindByShortName("Request");
	httpRequest.Add(oce.FindByShortName("WebRequest"));
	CxList methods = Find_Methods() * XSAll;
	CxList request = methods.FindByShortName("request");

	request -= request.DataInfluencedBy(XSAll.FindByShortName("readDestination"));
	result.Add(XSAll.GetParameters(request, 1));
	result.Add(XSAll.GetParameters(httpRequest, 1));
	result -= XSAll.FindByMemberAccess("headers.set");
}