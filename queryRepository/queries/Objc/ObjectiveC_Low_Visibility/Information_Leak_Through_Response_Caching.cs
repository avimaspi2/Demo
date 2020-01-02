//SAP Improvment informationLeakResponseCaching

//Ignoring cache via the AFHTTPRequestOperation object (application level)
CxList requestSerializer = All.FindByMemberAccess("AFHTTPRequestOperationManager.requestSerializer").GetMembersOfTarget();

//Checking if AFHTTPRequestOperationManager framework exist with the ignorCaching 
CxList AfhttpFramework = All.GetParameters(requestSerializer)
	.FindByType(typeof(Param))
	.FindByShortName("NSURLRequestReloadIgnoringLocalCacheData");
	
//If AFHTTPRequestOperationManager ignorCache mechanism not exist globally 
//We try to find cases where ignoring the cache locally on each request specifically
if (AfhttpFramework.Count == 0)
{
	//Find all requests (outputs) that are actually triggered and generating response
	CxList requests = All.FindByMemberAccess("NSMutableURLRequest.*");
	requests.Add(All.FindByMemberAccess("NSURLRequest.*"));
	requests.Add(All.FindByMemberAccess("URLRequest.*"));

	//Find the NSURLConnection object that fire the request trough the channel
	CxList responses = All.FindByMemberAccess("NSURLConnection.*");

	//Find Sanitizers
	//This is done in order to get the request parent that has ignoreCachePolicy
	//Ignoring cache on the request object itself
	CxList sanitizers = All.GetParameters(requests)
		.FindByType(typeof(Param))
		.FindByShortName("NSURLRequestReloadIgnoringCacheData")
		.GetAncOfType(typeof(MethodInvokeExpr)).GetTargetOfMembers(); 


	result = requests.InfluencingOnAndNotSanitized(responses, sanitizers);
}