CxList regexMembers = Find_Regex();

result.Add(All.FindByMemberAccess("Regex.Match", true));
result.Add(All.FindByMemberAccess("Regex.IsMatch", true));
result.Add(All.FindByMemberAccess("Regex.Matches", true));
result.Add(All.FindByMemberAccess("Regex.Replace", true));
result.Add(All.FindByMemberAccess("Regex.Split", true));

result = result.GetMembersWithTargets(regexMembers);