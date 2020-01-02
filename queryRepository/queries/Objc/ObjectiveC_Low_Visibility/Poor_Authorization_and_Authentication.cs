// Query Poor_Authorization_and_Authentication 
// -----------------------
// This issue is mainly in server side authentication. 
// For mobile apps, we will check if any identifiers (IP, MAC, UDID) are sent.
//

CxList outputs = Find_Interactive_Outputs();
CxList UDID = All.FindByMemberAccess("UIDevice.currentDevice");
UDID = UDID.GetMembersOfTarget().FindByShortName("uniqueIdentifier");
CxList IP = All.FindByShortName("sin_addr").GetAncOfType(typeof(MethodInvokeExpr));
IP = IP.FindByShortName("inet_ntoa");

CxList identifiers = All.NewCxList();
identifiers.Add(UDID);
identifiers.Add(IP);
identifiers.Add(All.FindByShortName("*MacAddress*", false));

result = identifiers.InfluencingOnAndNotSanitized(outputs, Find_General_Sanitize());