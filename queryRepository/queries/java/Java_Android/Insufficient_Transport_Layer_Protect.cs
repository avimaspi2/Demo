// Query Insufficient_Transport_Layer_Protect
// ------------------------------------------------ 
// Vulnerability: Insecure data transfer in transport layer
// Description: After data has been securely saved on the device the next 
//              high concern area is protecting the communications between 
//              the mobile application and the server.

// The purpose of the query is to find the scenarios of using HTTP protocol 
// instead of HTTPS

bool isSanitized = Find_Android_Transport_Layer_Sanitize().Count > 0;

if(!isSanitized)
{
	//The block below finds access to the network over HTTP and not HTTPS
	CxList pureHTTP = Find_Pure_http(); 

	CxList write = Find_Write();
	write.Add(Find_Request());
	result = write.DataInfluencedBy(pureHTTP);
}