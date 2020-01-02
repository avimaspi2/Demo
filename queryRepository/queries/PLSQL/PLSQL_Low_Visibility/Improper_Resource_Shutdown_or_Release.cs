CxList open_resource = 
	All.FindByMemberAccess("UTL_FILE.FOPEN", false) +
	All.FindByMemberAccess("UTL_TCP.OPEN_CONNECTION", false) +
	All.FindByMemberAccess("UTL_SMTP.OPEN_CONNECTION", false);
open_resource *= Find_Methods();

CxList close_resource = 
	All.FindByMemberAccess("UTL_FILE.FCLOSE", false) +
	All.FindByMemberAccess("UTL_TCP.CLOSE_CONNECTION", false) +
	All.FindByMemberAccess("UTL_SMTP.CLOSE_CONNECTION", false);

CxList close_all_resources = 
	All.FindByMemberAccess("UTL_FILE.FCLOSE_ALL", false) + 
	All.FindByMemberAccess("UTL_TCP.CLOSE_ALL_CONNECTIONS", false) +
	All.FindByMemberAccess("UTL_SMTP.CLOSE_ALL_CONNECTIONS", false);

CxList closing_all = All.FindAllReferences(close_all_resources.GetTargetOfMembers()).GetMembersOfTarget();
closing_all *= open_resource;

result = open_resource - open_resource.DataInfluencingOn(close_resource) - closing_all;