// Query Insufficient_Transport_Layer_Protect
// //////////////////////////////////////////////// 
// Vulnerability: Insecure data transfer in transport layer
// Description: After data has been securely saved on the device the next 
//              high concern area is protecting the communications between 
//              the mobile application and the server.

// The purpose of the query is to find the scenarios of using HTTP protocol 
// instead of HTTPS where sensitive data is transferred

CxList gpsData = All.FindByTypes(new String[] {
	"GeoCoordinate",
	"Geoposition"});

CxList contacts = All.FindByType("Contact");

CxList pureHttp = Find_Pure_http(); 
CxList httpWrite = All.FindByMemberAccess("*Stream.Write").DataInfluencedBy(pureHttp);
httpWrite.Add(All.FindByMemberAccess("*HttpClient.PostAsync").DataInfluencedBy(pureHttp));
httpWrite.Add(All.FindByMemberAccess("*HttpClient.PutAsync").DataInfluencedBy(pureHttp));

CxList personal = All.NewCxList();
personal.Add(Find_All_Passwords());
personal.Add(Find_Personal_Info());
personal.Add(gpsData);
personal.Add(contacts);

CxList encrypt = Find_Encrypt();

result = httpWrite.InfluencedByAndNotSanitized(personal, encrypt);


//handle https with filters

CxList iscAdd = All.FindByMemberAccess("IgnorableServerCertificateErrors.Add");
CxList client = All.FindByType("HttpClient");
CxList postAsyncCall = client.GetMembersOfTarget().FindByShortName("PostAsync").GetTargetOfMembers();
CxList putAsyncCall = client.GetMembersOfTarget().FindByShortName("PutAsync").GetTargetOfMembers();
CxList calls = postAsyncCall + putAsyncCall;
CxList clientWithIgnoredFilter = calls.DataInfluencedBy(iscAdd);
clientWithIgnoredFilter = clientWithIgnoredFilter.GetMembersOfTarget();
clientWithIgnoredFilter = clientWithIgnoredFilter.DataInfluencedBy(All.FindByShortName("https:*"));
clientWithIgnoredFilter = clientWithIgnoredFilter.InfluencedByAndNotSanitized(personal,encrypt);

result.Add(clientWithIgnoredFilter);