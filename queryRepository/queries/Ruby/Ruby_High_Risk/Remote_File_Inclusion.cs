CxList methods = Find_Methods();
CxList include = methods.FindByShortName("load", false) +
	methods.FindByShortName("require", false) +
	methods.FindByShortName("require_relative", false);
include -= include.GetTargetOfMembers().GetMembersOfTarget();

result = include.DataInfluencedBy(Find_Interactive_Inputs());