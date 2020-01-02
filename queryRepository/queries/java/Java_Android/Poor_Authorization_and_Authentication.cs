// Query Poor_Authorization_and_Authentication (Strong_Authentication_Failure)
// -----------------------------------
// Vulnerability: Failure to protect resources with strong authentication 
// This query finds the places where the application performs a login according to device ID


// Send DeviceID info over network
CxList http = All.FindByName(@"*http*");
CxList findWrite = Find_Write();
CxList outInfluencedByHttp = findWrite * findWrite.DataInfluencedBy(http);
CxList deviceIDInfo = All.FindByMemberAccess("TelephonyManager.getDeviceId");										  
result = deviceIDInfo.DataInfluencingOn(outInfluencedByHttp);