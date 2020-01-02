CxList methods = Find_Methods();

result.Add(All.FindByType("MD5", false).GetMembersOfTarget().FindByShortName("ComputeHash", false));
result.Add(methods.FindByMemberAccess("MD5CryptoServiceProvider.ComputeHash", false));
result.Add(All.FindByType("SHA1", false).GetMembersOfTarget().FindByShortName("ComputeHash", false));
result.Add(methods.FindByMemberAccess("SHA1CryptoServiceProvider.ComputeHash", false));