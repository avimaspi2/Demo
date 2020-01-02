CxList regex = Find_Regex();

List<String> pattern = new List<String>(){ "match", "ismatch" };
result = Find_Methods().FindByShortNames(pattern, false).DataInfluencedBy(regex);
result.Add(regex.GetMembersOfTarget().FindByShortNames(pattern, false));