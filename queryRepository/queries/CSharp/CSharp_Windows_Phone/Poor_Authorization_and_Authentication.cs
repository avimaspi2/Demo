// Query Poor_Authorization_and_Authentication (Strong_Authentication_Failure)
// //////////////////////////////////-
// Vulnerability: Failure to protect resources with strong authentication 
// This query finds the places where the application performs a login according to device ID

// Find device ID
CxList deviceExtendedProperties = All.FindByMemberAccess("DeviceExtendedProperties.TryGetValue");
CxList deviceIdPropertyName = All.FindByShortName("DeviceUniqueId");
CxList deviceIdFunc = deviceExtendedProperties.FindByParameters(deviceIdPropertyName);
CxList deviceId = All.GetParameters(deviceIdFunc, 1);
// Another method to get device ID
CxList deviceId2 = All.FindByMemberAccess("DeviceExtendedProperties.GetValue");
deviceId.Add(deviceId2);

// Send DeviceID info over network
CxList http = All.FindByName(@"*http*");
CxList outInfluencedByHttp = Find_Write() * Find_Write().DataInfluencedBy(http);
								  
result = deviceId.DataInfluencingOn(outInfluencedByHttp);