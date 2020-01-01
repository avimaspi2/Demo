// Query General_Android_Find_Request_Permissions
// ----------------------------------------------
// The purpose of this query is to find all permissions required by an application
// This is NOT a vulnerability list, it allows a customer to verify which resources are 
// required by a 3d party application.
result = All.FindByShortName("android.permission.*");

// Check for runtime permissions
CxList membersReqPermission = Find_Methods().FindByMemberAccess("ActivityCompat.requestPermissions", true);
result.Add(Find_MemberAccess().FindByMemberAccess("Manifest.permission").GetMembersOfTarget().GetByAncs(membersReqPermission));