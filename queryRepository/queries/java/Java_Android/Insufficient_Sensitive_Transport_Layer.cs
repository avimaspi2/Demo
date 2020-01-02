// Query Insufficient_Sensitive_Transport_Layer
// -------------------------------------------
// Vulnerability: Personal information insecurely transfered via the transport layer
// Description: After data has been securely saved on the device the next 
//              high concern area is protecting the communications between 
//              the mobile application and the server.

// The purpose of the query is to find the scenarios of using HTTP protocol 
// instead of HTTPS where sensitve data is transfered

//Check manifest for sanitization

bool isSanitized = Find_Android_Transport_Layer_Sanitize().Count > 0;

if(!isSanitized)
{
	//The block below finds access to the network over HTTP and not HTTPS
	CxList pureHTTP = Find_Pure_http();
	pureHTTP.Add(All.FindByType("HttpURLConnection"));
	

	// Find outputs that performed over HTTP
	CxList write = Find_Write();
	
	write.Add(Find_Request());	
	CxList outInfluencedByHttp = write * write.DataInfluencedBy(pureHTTP);
	
	//support HTTPClient
	CxList httpClient = All.FindByMemberAccess("*HttpClient.execute");
	CxList sslSanitized = httpClient.DataInfluencedBy(All.FindByShortName("ssl*", false));
	CxList nonSanitizedClient = httpClient - sslSanitized;
	outInfluencedByHttp.Add(nonSanitizedClient);	
	
	//support OKHttpClient
	CxList okHttpClient = All.FindByMemberAccess("OkHttpClient.newCall");

	CxList tlsSanitization = All.FindByMemberAccess("ConnectionSpec.MODERN_TLS");
	tlsSanitization.Add(All.FindByMemberAccess("ConnectionSpec.COMPATIBLE_TLS"));
	CxList tlsSanitized = okHttpClient.DataInfluencedBy(tlsSanitization);
	CxList notSanitizedOkHttpClient=okHttpClient -tlsSanitized;
	outInfluencedByHttp.Add(notSanitizedOkHttpClient);
	
	 

	// Find all paths that include sensitive (personal) data and outstreamed over HTTP
	CxList pathResult = Find_Personal_Info().DataInfluencingOn(outInfluencedByHttp);
	result = pathResult.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
	result.Add(Find_Volley_Insufficient_Sensitive_Transport_Layer());
}