CxList regex = Find_Regex();

result = Find_Methods().FindByShortName("match", false).DataInfluencedBy(regex) +
	regex.GetMembersOfTarget().FindByShortName("match", false) +
	Find_Methods().FindByShortName("ismatch", false).DataInfluencedBy(regex) +
	regex.GetMembersOfTarget().FindByShortName("ismatch", false);