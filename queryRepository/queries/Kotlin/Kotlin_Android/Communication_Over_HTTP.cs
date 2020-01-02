// This query tries to find ocurrences of pure http requests

// Checks if we have a generic sanitizer (cleartextTrafficPermitted=false in android networkSecurityConfig)
CxList sanitize = Find_Android_Transport_Layer_Sanitize();

if (sanitize.Count == 0) {
	CxList httpStrings = Find_Pure_http();
	CxList requests = Find_Remote_Requests() - Find_Remote_Requests_HTTPS_Sanitize();
	result = requests.DataInfluencedBy(httpStrings).ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);
}