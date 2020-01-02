//finds Zend unique interactive outputs
//finds all DOM relevant response modification
if (Find_Extend_Zend().Count > 0)
{
	CxList methods = Find_Methods();

	CxList memberAccess = All.FindByType(typeof(MemberAccess));
	CxList access = methods + memberAccess;
	CxList zendControllerResponse = access.FindByShortNames(new List<string> {
		
		"sendResponse",		
		"appendBody",
		"setBody",
		"setHeader",
		"setRawHeader",
		"setRedirect",
		"sendHeaders",
		"prepend"});
/*
		access.FindByShortName("sendResponse") +		
		access.FindByShortName("appendBody") +
		access.FindByShortName("setBody") +
		access.FindByShortName("setHeader") +
		access.FindByShortName("setRawHeader") +
		access.FindByShortName("setRedirect") +
		access.FindByShortName("sendHeaders") +
		access.FindByShortName("prepend");
*/	
	
	CxList responseParams = 
		memberAccess.FindByShortName("body") +
		memberAccess.FindByShortName("header*");
	
	zendControllerResponse.Add(responseParams);
	result.Add(zendControllerResponse);
	
}