// Find_Web_Services
// _________________
//
// The purpose of the query to find Web Services that can be used for input or output

CxList webClientProtocol = All.InheritsFrom("SoapHttpClientProtocol");	
CxList webServices = All.FindByType(webClientProtocol) + All.FindByType("*WebService");
result = webServices;