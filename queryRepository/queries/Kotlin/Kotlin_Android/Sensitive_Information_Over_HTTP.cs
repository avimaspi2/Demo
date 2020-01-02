// This query tries to find ocurrences of pure http requests using sensitive information (ex passwords)

// Checks if we have a generic sanitizer (cleartextTrafficPermitted=false in android networkSecurityConfig)
CxList sanitize = Find_Android_Transport_Layer_Sanitize();

if (sanitize.Count == 0) {
	CxList sensitiveFields = Find_Personal_Info();
	if (Find_Android_Settings().Count > 0) {
		sensitiveFields.Add(Find_Android_Sensitive());
	}
	
	sensitiveFields -= Find_String_Literal();
	
	CxList httpReferences = Find_Pure_http();
	CxList httpRequests = (Find_Remote_Requests() - Find_Remote_Requests_HTTPS_Sanitize()).DataInfluencedBy(httpReferences);
	result = httpRequests.DataInfluencedBy(sensitiveFields);
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);
	
}