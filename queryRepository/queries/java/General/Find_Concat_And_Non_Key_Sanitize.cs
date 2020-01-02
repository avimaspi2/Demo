CxList concat = All.FindByShortName("concatenate", false);
CxList nonKey = All.FindByMemberAccess("SecretKeyFactory.getInstance");
nonKey.Add(All.GetParameters(All.FindByMemberAccess("String.split")));

result = concat;
result.Add(nonKey);